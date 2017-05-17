package com.example.utoaandroidnative;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;

import com.liangddyy.UToAUnity.UnityPlayerActivity;

public class MainActivity extends AppCompatActivity {
    private Button btnToUnity;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        btnToUnity = (Button) findViewById(R.id.btn_to_unity);
        btnToUnity.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Intent intent = new Intent(MainActivity.this, ExtendActivity.class);
                startActivity(intent);
            }
        });
    }
}
