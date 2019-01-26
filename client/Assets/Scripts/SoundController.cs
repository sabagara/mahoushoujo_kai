using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundController {
	private static SoundController m_Instance;

	private AudioSource m_SeAudioSource;
	private AudioSource m_BgmAudioSource;

	private Dictionary<SOUND, List<AudioClip>> audioClips = new Dictionary<SOUND, List<AudioClip>>();


	public enum SOUND {
		MAINCHARACTOR_DASH, //主人公ダッシュ
		MAINCHARACTOR_GET_PANTY, // 主人公パンツ獲得
		MAINCHARACTOR_POWERUP, // 主人公パワーアップアイテム獲得
		MAINCHARACTOR_JUMP, // 主人公ジャンプ
		MAINCHARACTOR_LANDING, // 主人公ジャンプ着地
		MAINCHARACTOR_JOIN_PANTY, //お告げシーン 装着

		GAME_START, // ゲームスタート
		GAME_OVER, // ゲームオーバー
		GAME_CLEAR, // ゲームクリア

		BGM_TITLE, // BGM(タイトル)
		BGM_MAIN, // BGM(メイン)

		// 主人公ボイス群
		VOICE_MAIN_START, // ゲームスタート時
		VOICE_MAIN_CLEAR, // ゲームクリア時
		VOICE_MAIN_JUMP, // ジャンプ
		VOICE_MAIN_GET_PANTY,  // パンツ獲得時

		// 市民ボイス
		VOICE_CITIZEN_ON_ENCOUNT, // 事案遭遇時

	};

	private SoundController()
	{
		addAudio (SOUND.MAINCHARACTOR_DASH, "Sounds/dash-asphalt1");
		addAudio (SOUND.MAINCHARACTOR_GET_PANTY, "Sounds/page10_get_panty");
		addAudio (SOUND.MAINCHARACTOR_POWERUP, "Sounds/powerup");
		addAudio (SOUND.MAINCHARACTOR_JUMP, "Sounds/jump");
		addAudio (SOUND.MAINCHARACTOR_LANDING, "Sounds/landing");
		addAudio (SOUND.MAINCHARACTOR_JOIN_PANTY, "Sounds/join");

		addAudio (SOUND.GAME_START, "Sounds/game_start");
		addAudio (SOUND.GAME_OVER, "Sounds/game_over");
		addAudio (SOUND.GAME_CLEAR, "Sounds/game_clear");

		addAudio (SOUND.BGM_TITLE,"Sounds/quiet-residential-area1");
		addAudio (SOUND.BGM_MAIN,"Sounds/quiet-residential-area1");


		// ボイス群 ここは複数パターンあり
		addAudio (SOUND.VOICE_MAIN_START, "Sounds/mainchara_gamestart_pattern1_1");
		addAudio (SOUND.VOICE_MAIN_START, "Sounds/mainchara_gamestart_pattern3_1");
		addAudio (SOUND.VOICE_MAIN_CLEAR, "Sounds/mainchara_gameclear_pattern1_1");

		addAudio (SOUND.VOICE_MAIN_JUMP, "Sounds/mainchara_jump_pattern1_1");
		addAudio (SOUND.VOICE_MAIN_GET_PANTY, "Sounds/mainchara_getpanty_pattern1_1");
		addAudio (SOUND.VOICE_MAIN_GET_PANTY, "Sounds/mainchara_getpanty_pattern1_2");
		addAudio (SOUND.VOICE_MAIN_GET_PANTY, "Sounds/mainchara_getpanty_pattern1_3");
		addAudio (SOUND.VOICE_MAIN_GET_PANTY, "Sounds/mainchara_getpanty_pattern2_1");
		addAudio (SOUND.VOICE_MAIN_GET_PANTY, "Sounds/mainchara_getpanty_pattern2_2");
		addAudio (SOUND.VOICE_MAIN_GET_PANTY, "Sounds/mainchara_getpanty_pattern2_3");
		addAudio (SOUND.VOICE_MAIN_GET_PANTY, "Sounds/mainchara_getpanty_pattern3_1");
		addAudio (SOUND.VOICE_MAIN_GET_PANTY, "Sounds/mainchara_getpanty_pattern3_2");

		addAudio (SOUND.VOICE_CITIZEN_ON_ENCOUNT, "Sounds/citizen_watch_pattern1_1");
		addAudio (SOUND.VOICE_CITIZEN_ON_ENCOUNT, "Sounds/citizen_watch_pattern1_2");
		addAudio (SOUND.VOICE_CITIZEN_ON_ENCOUNT, "Sounds/citizen_watch_pattern2_1");
		addAudio (SOUND.VOICE_CITIZEN_ON_ENCOUNT, "Sounds/citizen_watch_pattern2_2");
		addAudio (SOUND.VOICE_CITIZEN_ON_ENCOUNT, "Sounds/citizen_watch_pattern3_1");
		addAudio (SOUND.VOICE_CITIZEN_ON_ENCOUNT, "Sounds/citizen_watch_pattern3_2");




	}

	public static SoundController Instance {
		get {
			if( m_Instance == null ) m_Instance = new SoundController();
			return m_Instance;
		}
	}

	// AudioClipsを追加する。重複判定しないので注意
	private void addAudio(SOUND soundKey, string filePath) {
		List<AudioClip> sounds;

		if (!audioClips.ContainsKey (soundKey)) {
			sounds = new List<AudioClip> ();
			audioClips.Add (soundKey, sounds);
		} else {
			sounds = audioClips [soundKey];
		}

		//Debug.Log ("sounds : " + sounds.Count);
		sounds.Add ((AudioClip)Resources.Load (filePath));

		//audioClips.Add (soundKey, (AudioClip)Resources.Load (filePath));
	}

	// AudioClipを取得する
	// soundKeyに対して複数のAudioClipが存在する場合、ランダムで1つ取得する
	private AudioClip getAudio(SOUND soundKey){
		if(!audioClips.ContainsKey(soundKey)){
			return null;
		}
		List<AudioClip> sounds = audioClips [soundKey];

		int selectedIndex = Mathf.FloorToInt(Random.Range (0, sounds.Count -1 ));

		//Debug.Log ("soundKey : " + soundKey  + " / selectedIndex : " + selectedIndex +" / maxIndex : " + sounds.Count);
		return sounds[selectedIndex];
	}

	private void initializeAudioSource(){
		if (!GameObject.Find ("SoundPlayer")) {
			GameObject soundPlayer = new GameObject ("SoundPlayer"); 
			m_SeAudioSource = soundPlayer.AddComponent<AudioSource> ();
			m_BgmAudioSource = soundPlayer.AddComponent<AudioSource> ();
			// BGMのループ再生を行う
			m_BgmAudioSource.loop = true;

		}
	}

	// ループあり再生(BGM)
	public void playBgm(SOUND soundName){
		AudioClip audioClip = getAudio (soundName);
		if (audioClip == null) {
			return;
		}

		initializeAudioSource ();

		m_BgmAudioSource.clip = audioClip;
		m_BgmAudioSource.Play ();
	}

	// ループ無し再生(SE)
	public void play(SOUND soundName) {
		AudioClip audioClip = getAudio (soundName);
		if (audioClip == null) {
			return;
		}

		initializeAudioSource ();

		m_SeAudioSource.PlayOneShot(audioClip);	
	}
}

