using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

// the JigsawPuzzle Class is the base class that can be used to create an in-game jigsaw puzzle
//
// the normal procedure would be to create a custom JigsawPuzzle subclass
// and override the following base functions :
//
// - PuzzleStart(); 						: is called when new puzzle is started
// - ActivatePiece(GameObject piece); 		: is called when a 'loose' puzzle piece is selected (start drag)
// - DeactivatePiece(GameObject piece);		: is called when a 'loose' puzzle piece is released (stop drag)
// - PiecePlaced(GameObject piece); 		: is called when a puzzle piece is placed on the puzzle on the right spot
// - PuzzleSolved(int moves, float time);	: is called when the puzzle has been solved
// 
// (see DemoJigsawPuzzle class included with the product Demo)
//
[Serializable]
public class JigsawPuzzle : MonoBehaviour
{
    private Vector3 piecesOldPosition;
    private readonly Hashtable piecePositions = new Hashtable();
    private readonly ArrayList pieces = new ArrayList();
    private readonly Hashtable piecesLookup = new Hashtable();
    private GameObject activePiece;
    private Vector3 activePoint;
    private Vector2 checkContainerSize;
    private bool checkedOnce;
    private Vector2 checkSize;
    private string checkTopLeftPiece = "";
    private bool dragging;
    private Touch dragTouch;
    // ---------------------------------------------------------------------------------------------------------
    // public (published) attributes - these can be set after adding the 
    // script to a container (like a cube primitive)
    // ---------------------------------------------------------------------------------------------------------

    public Texture image = null; // will contain the jigsaw projected picture
    private GameObject linesH;
    private GameObject linesV;
    // ---------------------------------------------------------------------------------------------------------
    // private attributes
    // ---------------------------------------------------------------------------------------------------------

    //private GameObject testContainer;
    private JigsawMain main;
    private GameObject pieceCache;
    private GameObject piecesContainer;

    public int placePrecision = 12;
    // how precise must a piece beeing placed on the puzzle ( = percentage | 6 - 15 are good values , maybe higher up to 25 for small children )

    private GameObject puzzleContainer;
    private int puzzleMode;
    private int puzzleMoves;
    private GameObject puzzlePlane;
    private int puzzleTicks;
    private bool restarting;
    public bool rotatePieces = false; // true to add rotation support - NOT IMPLEMENTED YET
    private GameObject sampleImage;

    public bool scatterPieces = true;
    // scatter pieces when puzzle starts - if false the pieces are placed on their spot on the puzzle

    public bool showImage = true; // display 'helper' semi-transparant - greyscale sample picture
    public bool showLines = true; // display 'helper' puzzle matrix 
    public Vector2 size = new Vector2(5, 5); // how many pieces will this puzzle have (x,y)
    public float spacing = 0.01f;

    public string topLeftPiece = "11";
    // topleft piece - format YX (1,2,3,4,5) so 11 to 55 - 25 unique start possiblities

    // ---------------------------------------------------------------------------------------------------------
    // protected attributes (accessable in 'custom' subclass)
    // ---------------------------------------------------------------------------------------------------------

    // number of pieces of current puzzle
    protected int pieceCount
    {
        get { return (int)(size.x * size.y); }
    }

    // number of pieces placed in current puzzle

    protected int piecesPlaced
    {
        get { return puzzleContainer.transform.GetChildCount(); }
    }

    // number of pieces to go in current puzzle
    protected int piecesScattered
    {
        get { return piecesContainer.transform.GetChildCount(); }
    }

    // puzzle progress - percentage 0-100
    protected float puzzleProgress
    {
        get { return (piecesPlaced / pieceCount) * 100; }
    }

    //private ArrayList piecesOldRotation = new ArrayList();
    //private Hashtable  testHash = new Hashtable();

    // ---------------------------------------------------------------------------------------------------------
    // virtual methods
    // ---------------------------------------------------------------------------------------------------------

    // This will be called when a new puzzle has been started
    // you need to override this method when you create your 'custom' subclass
    protected virtual void PuzzleStart()
    {
    }

    // This will be called when a 'loose' puzzle piece has been selected (start drag)
    // you need to override this method when you create your 'custom' subclass
    protected virtual void ActivatePiece(GameObject piece)
    {
    }

    // This will be called when a puzzle piece has been released (stop drag)
    // you need to override this method when you create your 'custom' subclass
    protected virtual void DeactivatePiece(GameObject piece)
    {
    }

    // This will be called when a piece has been placed on the puzzle on the correct spot
    // you need to override this method when you create your 'custom' subclass
    protected virtual void PiecePlaced(GameObject piece)
    {
    }

    // This will be called when the puzzle has been solved
    // you need to override this method when you create your 'custom' subclass
    protected virtual void PuzzleSolved(int moves, float time)
    {
    }

    // this will be called when a piece is about to be scattered - if null is returned the default scattering is performed
    // you need to override this method when you create your 'custom' subclass
    protected virtual GameObject ScatterPiece(GameObject piece)
    {
        return null;
    }

    // ---------------------------------------------------------------------------------------------------------
    // methods
    // ---------------------------------------------------------------------------------------------------------

    // restart the current puzzle
    public void Restart()
    {
        // set indicator that puzzle has to be restarted - this is picked up in the Update() - process
        restarting = true;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (main == null)
        {
            // main puzzle initialization
            if (!checkedOnce)
            {
                // check ONCE if JigsawMain Script is found on JisawMain Prefab
                var go = GameObject.Find("JigsawMain");
                if (go == null)
                {
                    Debug.LogError("JigsawMain (prefab) GameObject not added to scene!");
                    checkedOnce = true;
                    return;
                }
                // get JigsawMain class for piece prototype access
                main = go.GetComponent("JigsawMain") as JigsawMain;
                // check if main is initialized correctly so isValid should be true
                if (main != null)
                    if (!main.isValid)
                    {
                        Debug.LogError(
                            "JigsawMain (prefab) GameObject is not valid - Base puzzle pieces could not be found!");
                        main = null;
                        checkedOnce = true;
                        return;
                    }

                // check if TopLeftPiece Exists , if not we will take '11'
                if (main.GetBase(topLeftPiece) == null)
                    topLeftPiece = "11";

                // initialization of this puzzle
                // create horizontal and vertical lines
                SetLines();
                // create sample image
                SetSample();
                // create pieces of this puzzle
                SetPieces(false);
                // create mouse control 'hit' plane
                SetPlane();
            }
            else
                // puzzle system is invalid so return;
                return;
        }
        else
        {
            // JigsawMain was found and is valid so we can go on with our puzzle	
            // Lets first check if base puzzle settings have been changed like size or the top-left piece so we have to force a restart
            if (!Equals(size, checkSize) || topLeftPiece != checkTopLeftPiece || restarting)
            {
                if (activePiece)
                {
                    // deactivate the current active piece
                    DeactivatePiece(activePiece);
                    activePiece = null;
                }
                // base puzzle settings have been changed so reset lines, sample image and pieces.
                SetLines();
                SetSample();
                SetPieces(true);
                // restart puzzle - so puzzleMode to 0
                puzzleMode = 0;
            }

            // check if lines have to be shown/hidden
            if (linesH.active != showLines) linesH.active = showLines;
            if (linesV.active != showLines) linesV.active = showLines;
            // check if sample image has to be shown/hidden
            if (sampleImage.active != showImage) sampleImage.active = showImage;

            // Puzzle control
            switch (puzzleMode)
            {
                case 0: // puzzle initialization
                    if (pieceCount == 0) return;

                    if (scatterPieces)
                        // we have pieces so scatter them around
                        ScatterPieces();

                    if (rotatePieces)
                        // rotate the pieces on the scattered spot
                        RotatePieces();

                    // starting to puzzle so reset move count and puzzleTime
                    puzzleMoves = 0;
                    puzzleTicks = Environment.TickCount;
                    restarting = false;
                    // call overriden PuzzleStart function
                    PuzzleStart();
                    // Puzzle control to next step
                    puzzleMode++;
                    break;

                case 1: // we are puzzling

                    if (Input.GetMouseButton(0))
                    {

                        // only check position is mouse click or first touch is active
                        var position = Input.mousePosition;
                        if (Input.multiTouchEnabled)
                            position = Input.mousePosition;
                        //position = Input.touches[0].position;//修改

                        // left mouse button is down so check if we have an active piece
                        if (activePiece != null)
                        {
                            //拖拽中
                            // if we have an active piece we will move it, if we moved the mouse
                            RaycastHit hit;
                            // cast a ray from the camera to the mouse control hitplane
                            if (puzzlePlane.GetComponent<Collider>()
                                .Raycast(Camera.main.ScreenPointToRay(position), out hit,
                                    Vector3.Distance(Camera.main.transform.position, transform.position) * 2))
                            {
                                // calculate the distance between the previous hit.point and the current
                                var d = hit.point - activePoint;
                                // move active piece with the calculated distance
                                activePiece.transform.position += d;
                                // current point becomes new hit.point
                                activePoint = hit.point;
                            }
                        }
                        else
                        {

                            // no active piece so check if we can get one
                            // cast a ray from the camera collect all hits that correspond with the 'puzzle' layer mask (31 = default but can be set on JigSawMain class)
                            var hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(position),
                                Vector3.Distance(Camera.main.transform.position, transform.position) * 2,
                                1 << main.layerMask);
                            if (hits.Length > 0)
                            {
                                // we have hit something so check them
                                for (var h = 0; h < hits.Length; h++)
                                {
                                    if (hits[h].collider.gameObject == puzzlePlane)
                                        // if we hit the mouse control hit plane we register the hit.point for piece moving purposes
                                        activePoint = hits[h].point;
                                    else if (hits[h].collider.gameObject.transform.parent.gameObject == piecesContainer)
                                    {
                                        // we hit a piece so make it active if dont have one or if it is 'forward' the current active piece
                                        if ((activePiece != null &&
                                             activePiece.transform.position.z >
                                             hits[h].collider.gameObject.transform.position.z) || activePiece == null)
                                        {
                                            activePiece = hits[h].collider.gameObject;

                                            if (rotatePieces)
                                                //旋转到初始方向
                                                activePiece.transform.DORotate(new Vector3(0, 180, 0), 0.5f);

                                            piecesOldPosition = activePiece.transform.position;//保存鼠标位置
                                        }
                                    }
                                }
                                // active the piece if we have found one
                                if (activePiece != null)
                                {
                                    ActivatePiece(activePiece);
                                }
                                dragging = true;
                            }
                        }
                    }
                    else
                    {
                        dragging = false;
                        // mouse left button is not down
                        if (activePiece != null)
                        {
                            // if we have an active piece we have to deactivate it.
                            DeactivatePiece(activePiece);
                            // increase the number of moves
                            puzzleMoves++;

                            if (PieceInPlace())
                            {
                               
                                //当 拖拽到正确的位置时
                                // piece is released in the right spot so snap it into position
                                activePiece.transform.position =
                                    PiecePosition((Vector2)piecePositions[activePiece.name]);
                                // set parent to puzzleContainer so we lock it for dragging
                                activePiece.transform.parent = puzzleContainer.transform;
                                // call overriden PiecePlaced function
                                PiecePlaced(activePiece);

                                if (puzzleProgress == 100)
                                {
                                    // puzzle is solved so call overridden PuzzleSolved function
                                    PuzzleSolved(puzzleMoves, (Environment.TickCount - puzzleTicks) / 1000);
                                    puzzleMode++;
                                }
                            }else
                            {
                                //拼接错误时，移动到原来的位置
                                activePiece.transform.DOMove(piecesOldPosition, 0.5f);

                            }
                            activePiece = null;
                        }
                    }

                    break;
                case 2: // puzzle is Done - this a kind of sleep state.
                    break;
            }
        }
    }

    // Determine if active piece is in the right spot.
    // the placePrecision setting tells us how accurate we have to fit the piece on the puzzle
    // 12-15 is a good setting
    // 15-25 for young kids
    private bool PieceInPlace()
    {
        // call base piece size
        var dX = (transform.localScale.x / size.x);
        var dY = (transform.localScale.y / size.y);
        // calculate distance vector between active piece correct position and current position
        var dV = PiecePosition((Vector2)piecePositions[activePiece.name]) - activePiece.transform.position;
        // control vector is piece size
        var cV = new Vector3(dX, dY, 0);
        // check distance (with place precicion) from current position to correct position
        return ((Vector3.Distance(Vector3.zero, dV) / Vector3.Distance(Vector3.zero, cV) * 100) < placePrecision);
    }

    // Create horizontal lines to display on puzzle
    private void SetLinesHorizontal()
    {
        // we must have a valid topLeftPiece
        if (topLeftPiece.Length != 2) return;
        // get starting x-line from top left piece
        var tpX = Convert.ToInt32(topLeftPiece.Substring(1, 1));
        // get starting y-line from top left piece
        var tpY = Convert.ToInt32(topLeftPiece.Substring(0, 1));
        // we will recreate so destroy if we already have lines
        if (linesH != null) Destroy(linesH);
        // create a cube primitive for these lines
        linesH = GameObject.CreatePrimitive(PrimitiveType.Cube);
        linesH.name = "lines-horizontal";
        // add lines to puzzle
        linesH.transform.parent = gameObject.transform;
        // set 'transparent' material to lines horizontal
        linesH.GetComponent<Renderer>().material = main.linesHorizontal;
        // set the right scale (z = very thin) rotation and position so it will cover the puzzle
        linesH.transform.localScale = new Vector3(-1, -1 * (1 / size.y) * (size.y - 1), 0.0001F);
        linesH.transform.rotation = transform.rotation;
        // move this 'thin' cube so that it floats just above the puzzle
        linesH.transform.position = transform.position +
                                    transform.forward * ((transform.localScale.z / 2) + 0.001F);
        // scale the texture in relation to specified size
        linesH.GetComponent<Renderer>().material.mainTextureScale = new Vector2(-0.2F * size.x, -0.2F * (size.y - 1));
        // set the right offset in relation to the specified size and the specified topLeftPiece
        linesH.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(
            ((5 - size.x) * -0.2F) + ((tpX - 1) * 0.2F), 0.005F + ((tpY - 1) * -0.2F));
        linesH.active = false;
    }

    // Create vertical lines to display on puzzle
    private void SetLinesVertical()
    {
        // we must have a valid topLeftPiece
        if (topLeftPiece.Length != 2) return;
        // get starting x-line from top left piece
        var tpX = Convert.ToInt32(topLeftPiece.Substring(1, 1));
        // get starting y-line from top left piece
        var tpY = Convert.ToInt32(topLeftPiece.Substring(0, 1));
        // we will recreate so destroy if we already have lines
        if (linesV != null) Destroy(linesV);
        // create a cube primitive for these line
        linesV = GameObject.CreatePrimitive(PrimitiveType.Cube);
        linesV.name = "lines-vertical";
        // add lines to puzzle
        linesV.transform.parent = gameObject.transform;
        // set 'transparent' material to lines horizonta
        linesV.GetComponent<Renderer>().material = main.linesVertical;
        // set the right scale (z = very thin) rotation and position so it will cover the puzzl
        linesV.transform.localScale = new Vector3(-1 * (1 / size.x) * (size.x - 1), -1, 0.0001F);
        linesV.transform.rotation = transform.rotation;
        // move this 'thin' cube so that it floats just above the puzzle
        linesV.transform.position = transform.position +
                                    transform.forward * ((transform.localScale.z / 2) + 0.001F);
        // scale the texture in relation to specified size
        linesV.GetComponent<Renderer>().material.mainTextureScale = new Vector2(-0.2F * (size.x - 1), -0.2F * size.y);
        // set the right offset in relation to the specified size and the specified topLeftPiece
        linesV.GetComponent<Renderer>().material.mainTextureOffset =
            new Vector2(-0.2F * ((5 - size.x) + 1) + ((tpX - 1) * 0.2F), 0 + +((tpY - 1) * -0.2F));
        linesV.active = false;
    }

    private void SetLines()
    {
        // create puzzle lines
        SetLinesHorizontal();

        SetLinesVertical();
        // store size and top left piece so we can force a restart if they change
        checkSize = size;
        checkTopLeftPiece = topLeftPiece;
    }

    // rotate the pieces on their scattered spot
    private void RotatePieces()
    {
        // we will use the world space coordinates and scale of the puzzle for piece locations
        var s = transform.localScale;
        var dX = s.x / size.x;
        var dY = s.y / size.y;
        for (var p = 0; p < pieces.Count; p++)
        {
            var piece = pieces[p] as GameObject;

            //保存对象的旋转前的数据到hash表
            //testHash.Add(piece.name, piece.transform.rotation);

            //piecesOldRotation[p] = piece.transform.rotation;//保存 

            var parent = piece.transform.parent;
            var oldScale = piece.transform.localScale;
            piece.transform.parent = null;
            piece.transform.RotateAround(piece.transform.position + transform.right * -1 * (dX / 2) + transform.up * -1 * (dY / 2),
                transform.forward, Random.value * 360);
            piece.transform.parent = parent.transform;

            //            Vector3 pLoss = piece.transform.lossyScale;
            //            Vector3 panelLoss = parent.lossyScale;
            //            piece.transform.parent = parent;
            //		    piece.transform.localScale = new Vector3((pLoss.x/panelLoss.x), (pLoss.y/panelLoss.y),(pLoss.z/panelLoss.z));


            //piece.transform.rotation = Quaternion.Euler(0, 0, 30);

            //piece.transform.Rotate(piece.transform.position + transform.right * -1 * (dX / 2) + transform.up * -1 * (dY / 2), transform.forward, Random.value * 360)
            //		    piece.transform.parent = parent;
            //		    piece.transform.localPosition= trans.position;
            //		    piece.transform.localRotation = trans.rotation;
        }
    }

    // scatter the pieces and place them randomly around the puzzle
    private void ScatterPieces()
    {
        var piecesToScatter = new ArrayList(pieces);
        while (piecesToScatter.Count > 0)
        {
            // take a random piece 
            var piece = piecesToScatter[(int)(Mathf.Floor(Random.value * piecesToScatter.Count))] as GameObject;
            piecesToScatter.Remove(piece);

            // first try the custom scatter function
            if (ScatterPiece(piece) != null)
                continue;

            // will will determine in what rectangle a random position can be calculated for this piece
            var r = new Rect();

            // we will use the world space coordinates and scale of the puzzle for piece locations
            var p = transform.position;
            var s = transform.localScale;
            var dX = s.x / size.x;
            var dY = s.y / size.y;

            // si will hold the smallest scatter area size ( puzzle height/width devided by 4 `)
            var si = s.x / 4;
            if (s.y / 4 < si) si = s.y / 4;

            // determine randomly if the piece should be place above, to the left, to the right or below the puzzle
            switch ((int)(Mathf.Floor(Random.value * 4)) + 1)
            {
                case 1: // above
                    r = new Rect(p.x - (s.x / 2) - (dX / 2) + (s.x * 0.1f), p.y + (s.y / 2) + dY + si - (dY / 2), s.x * 0.8f,
                        -1 * (si - dY));
                    break;
                case 2: // right side
                    r = new Rect(p.x + (s.x / 2) + dX - (dX / 2), p.y + (s.y / 2) - (s.y * 0.1f), si - dX, -1 * s.y * 0.8f);
                    break;
                case 3: // below
                    r = new Rect(p.x - (s.x / 2) - (dX / 2) + (s.x * 0.1f), p.y - (s.y / 2) - (dY / 2), s.x * 0.8f, -1 * (si - dY));
                    break;
                case 4: // left side
                    r = new Rect(p.x - (s.x / 2) - dX - si + (dX / 2), p.y + (s.y / 2) - (s.y * 0.1f), si - dX, -1 * s.y * 0.8f);
                    break;
            }

            // because we used world coordinates we have to transfer the piece to the world by removing the parent
            //piece.transform.parent = null;
            // determine the random position with the valid rectangle
            piece.transform.parent = piecesContainer.transform;

            piece.transform.position =
                // start from puzzle position
                transform.position +
                // go to x of placement rectangle
                transform.right * -1 * r.xMin +
                // add random x position
                transform.right * -1 * Random.value * r.width +
                // go to top of placement rectangle
                transform.up * r.yMin +
                // add random y position
                transform.up * Random.value * r.height +
                // move to just 'forward' the surface of the puzzle cuve primitive
                transform.forward * ((transform.localScale.z / 2) + 0.001f) +
                // add a random forward value for better moving and selecting.
                transform.forward * (0.004f + (0.001F * Random.value * 20));

            if (transform.parent != null)
            {
                Vector2 vp = transform.parent.localToWorldMatrix.MultiplyPoint3x4(transform.localPosition);
                piece.transform.position -= (Vector3)vp;
            }
        }
    }

    // create mouse control hit plane
    private void SetPlane()
    {
        // Create Hit Plane Primitive GameObject for puzzle movement control
        puzzlePlane = GameObject.CreatePrimitive(PrimitiveType.Cube);
        puzzlePlane.name = "puzzlePlane";
        // position, rotate and scale it related to the puzzle (x/y scale x10)
        puzzlePlane.transform.parent = transform;
        puzzlePlane.transform.rotation = transform.rotation;
        puzzlePlane.transform.localScale = new Vector3(10, 10, 0.0001F);
        // let this hitplane float just 'forward' of the puzzle
        puzzlePlane.transform.position = transform.position +
                                         transform.forward * ((transform.localScale.z / 2) + 0.0004F);
        // set the layer mask for quick RayCasting
        puzzlePlane.layer = main.layerMask;
        // remove the renderer so we only use the collider
        Destroy(puzzlePlane.GetComponent("MeshRenderer"));
    }

    // create the sample image
    private void SetSample()
    {
        // if we already have one destroy it first
        if (sampleImage != null) Destroy(sampleImage);
        // create a primitive cube
        sampleImage = GameObject.CreatePrimitive(PrimitiveType.Cube);
        sampleImage.name = "sampleImage";
        // position, rotate and scale it related to the puzzle ( very thin )
        sampleImage.transform.parent = gameObject.transform;
        sampleImage.transform.localScale = new Vector3(1, 1, 0.0001F);
        sampleImage.transform.rotation = transform.rotation;
        sampleImage.transform.position = transform.position +
                                         transform.forward * ((transform.localScale.z / 2) + 0.0005F);
        // set image to puzzle material to sample image material (can be set on JigsawMain class)
        sampleImage.GetComponent<Renderer>().material = main.sampleImage;
        // set image to puzzle image
        sampleImage.GetComponent<Renderer>().material.mainTexture = image;
        sampleImage.GetComponent<Renderer>().material.mainTextureOffset = Vector2.zero;
        sampleImage.active = false;
    }

    // create piece containers
    private void CreateContainers()
    {
        if (piecesContainer != null) Destroy(piecesContainer);
        if (puzzleContainer != null) Destroy(puzzleContainer);
        if (pieceCache != null) Destroy(pieceCache);

        // piecesContainer will hold all 'loose' scattered pieces
        //testContainer = new GameObject("testContainer");
        piecesContainer = new GameObject("piecesContainer");
        //      避免 旋转式出现混乱。
        //        piecesContainer.transform.parent = gameObject.transform;
        //        piecesContainer.transform.rotation = transform.rotation;
        //        piecesContainer.transform.localScale = transform.localScale;
        //        piecesContainer.transform.position = transform.position;

        // puzzleContainer will hold all 'placed' pieces
        puzzleContainer = new GameObject("puzzleContainer");
        puzzleContainer.transform.parent = gameObject.transform;
        puzzleContainer.transform.rotation = transform.rotation;
        puzzleContainer.transform.localScale = transform.localScale;
        puzzleContainer.transform.position = transform.position;

        // pieceCache will hold all pieces that were created but no longer
        // are used on current puzzle - but can re-use after resize or restart
        pieceCache = new GameObject("pieceCache");
        pieceCache.transform.parent = gameObject.transform;
        pieceCache.transform.rotation = transform.rotation;
        pieceCache.transform.localScale = transform.localScale;
        pieceCache.transform.position = transform.position;
    }

    // get piece type (9) related to provided position on puzzle
    //
    // 	TL	T	TR
    //	L	C	R
    // 	BL	B	BR
    //
    private string GetType(Vector2 pos)
    {
        var x = pos.x;
        var y = pos.y;

        var pt = "C";
        if (y == 1)
        {
            if (x == 1) pt = "TL";
            else if (x == size.x) pt = "TR";
            else
                pt = "T";
        }
        else if (y == size.y)
        {
            if (x == 1) pt = "BL";
            else if (x == size.x) pt = "BR";
            else
                pt = "B";
        }
        else if (x == 1)
            pt = "L";
        else if (x == size.x)
            pt = "R";
        return pt;
    }

    // calculate right position for a x,y positioned piece on puzzle 
    private Vector3 PiecePosition(Vector3 pos)
    {
        var dX = transform.localScale.x / size.x;
        var dY = transform.localScale.y / size.y;

        // determine the position related x/y vector for this piece
        var positionVector =
            ((((transform.localScale.x / 2) * -1) + (dX * (pos.x - 1)) + (dX * (spacing / 2))) * transform.right * -1) +
            (((transform.localScale.y / 2)) - (dY * (pos.y - 1)) - (dY * (spacing / 2))) * transform.up;

        // set piece position to its right spot on the puzzle
        return transform.position +
               transform.forward * ((transform.localScale.z / 2) + 0.001f) +
               positionVector;
    }

    // initialize specific piece with right scale, position and texture (scale/offset)
    private void InitPiece(GameObject puzzlePiece, Vector2 pos)
    {
        // if we had a 5x5 puzzle we should scale the prototype pieces (from blender) like this
        //Vector3 scale5x5 = Vector3.Scale( new Vector3(0.1146f, 0.1146f, 0.25f), transform.localScale);
        var scale5x5 = Vector3.Scale(new Vector3(11.45f, 11.45f, 36.14547f), transform.localScale);
        // determine the puzzle size related scale vector
        var CxR = new Vector3(1 / (0.2F * size.x), 1 / (0.2F * size.y), 25 / (size.x * size.y));
        // set piece to world space so we can work with puzzle dimensions
        puzzlePiece.transform.parent = null;
        // set right scale for piece in world space
        puzzlePiece.transform.localScale = Vector3.Scale(scale5x5 * (1 - spacing), CxR);
        // rotate like puzzle
        puzzlePiece.transform.rotation = transform.rotation;
        // add piece to container
        puzzlePiece.transform.parent = piecesContainer.transform;
        // set piece position to its right spot on the puzzle
        puzzlePiece.transform.position = PiecePosition(pos);
        // add correct x,y position Vector2 to piecePositions for InPlace() control
        piecePositions.Add(puzzlePiece.name, pos);
        // we now are gonna work with local scale for the texture so 1 = puzzle width/height
        var scaleX = 1 / (0.2F * size.x);
        var scaleY = 1 / (0.2F * size.y);
        // set surface/image material to scatteredPieces material (can be set on JigsawMainClass)
        puzzlePiece.GetComponent<Renderer>().material = main.scatteredPieces;
        // set piece base material to pieceBase material (can be set on JigsawMainClass)
        puzzlePiece.GetComponent<Renderer>().materials[1] = main.pieceBase;
        // set surface texture to the puzzle image
        puzzlePiece.GetComponent<Renderer>().material.mainTexture = image;
        // scale the surface texture
        puzzlePiece.GetComponent<Renderer>().material.mainTextureScale = new Vector2(scaleX, scaleY);
        // determine the texture offset related to the size and piece position
        puzzlePiece.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0.2F * scaleX * (pos.x - 1),
            -0.2F * scaleY * (pos.y - 1 + (5 - size.y)));
    }

    // create a new piece from a specific prototype on a specific position with a specific piece type
    private GameObject CreateNewPiece(Vector2 piece, Vector2 pos, string pType)
    {
        GameObject puzzlePiece = null;
        // get piece prototype from main
        var basePiece = main.GetPiece("" + piece.y + "" + piece.x, pType);
        if (basePiece != null)
        {
            // prototype has been found so make an instance
            puzzlePiece =
                Instantiate(basePiece.gameObject, new Vector3(pos.x * 2F, pos.y * -2F, 0),
                    Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;
            // add collider to puzzle Pience
            puzzlePiece.AddComponent<BoxCollider>();
        }
        // add to specific layer for fast future RayCasting
        puzzlePiece.layer = main.layerMask;
        return puzzlePiece;
    }

    // Create or set (initialize) all pieces of the current puzzle
    private void SetPieces(bool recreate)
    {
        // we have to have a valid piece
        if (topLeftPiece.Length != 2) return;
        if (size.x <= 1 || size.y <= 1) return;

        if (!recreate)
            // only create piece containers the first time
            CreateContainers();
        else
        {
            // remove all active pieces from puzzle
            while (pieces.Count > 0)
            {
                var p = pieces[0] as GameObject;
                // create pieces array and positions
                pieces.Remove(p);
                piecePositions.Remove(p.name);
                p.active = false;
                // add piece to cache for re-use
                p.transform.parent = pieceCache.transform;
            }
        }

        // determine topleft piece x and y line
        var tpX = Convert.ToInt32(topLeftPiece.Substring(1, 1));
        var tpY = Convert.ToInt32(topLeftPiece.Substring(0, 1));
        var bX = tpX;

        var idX = 1;
        var idY = 1;

        // loop vertical rows of the puzzle
        for (var y = 1; y <= size.y; y++)
        {
            // loop horizontal columns of the puzzle
            for (var x = 1; x <= size.x; x++)
            {
                // get piece type of current position
                var pType = GetType(new Vector2(x, y));
                // check if specific piece was created earlier
                var puzzlePiece = piecesLookup["" + tpY + tpX + pType + "-" + idX] as GameObject;
                if (puzzlePiece != null)
                {
                    // puzzlePiece was created but can not be active, if so we have to increase the piece identifier index
                    // to find an inactive created piece
                    while (puzzlePiece != null && puzzlePiece.active)
                    {
                        idX++;
                        puzzlePiece = piecesLookup["" + tpY + tpX + pType + "-" + idX] as GameObject;
                    }
                }
                if (puzzlePiece != null)
                {
                    // a created piece has been found that can be used
                    puzzlePiece.name = "" + tpY + tpX + pType + "-" + idX;
                    // add puzzlePiece to this puzzle's pieces
                    InitPiece(puzzlePiece, new Vector2(x, y));
                    pieces.Add(puzzlePiece);
                    puzzlePiece.active = true;
                }
                else
                {
                    // create a new puzzlePiece
                    puzzlePiece = CreateNewPiece(new Vector2(tpX, tpY), new Vector2(x, y), pType);
                    puzzlePiece.name = "" + tpY + tpX + pType + "-" + idX;
                    if (puzzlePiece != null)
                    {
                        // add puzzlePiece to this puzzle's pieces and to lookup table
                        InitPiece(puzzlePiece, new Vector2(x, y));
                        piecesLookup.Add(puzzlePiece.name, puzzlePiece);
                        pieces.Add(puzzlePiece);
                    }
                }
                tpX++;
                if (tpX == bX + size.x || tpX == 6)
                {
                    if (tpX == 6)
                    {
                        tpX = 1;
                        idX++;
                    }
                    else
                        tpX = bX;
                }
            }
            tpX = bX;
            idX = 1;
            tpY++;
            if (tpY == 6)
            {
                tpY = 1;
                idY++;
            }
        }
    }
}