#pragma once
#include"Sprite.h"
#include "Audio.h"
class TitlePushStart : public Sprite
{
private:
	float m_timer = 0.0f; //点滅のタイマー
	Audio* m_clickse;
public:
	//更新処理だけオーバーライドする
	//初期化、終了。描画はSpriteの処理をそのまま使う
	void Update() override;
};

