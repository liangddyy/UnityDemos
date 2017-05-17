package com.example.utoaandroidnative;
import android.os.Bundle;
import android.view.KeyEvent;

import com.unity3d.player.UnityPlayerActivity;

/**
 * Created by liang on 2017/1/13.
 */

public class ExtendActivity extends UnityPlayerActivity {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }


    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event) {
        if (keyCode == KeyEvent.KEYCODE_BACK) {
            //sendBroadcast(new Intent("finish"));
            finish();
            mUnityPlayer.quit();
//                Log.e("key", "key");
        }
        return super.onKeyDown(keyCode, event);
    }
}
