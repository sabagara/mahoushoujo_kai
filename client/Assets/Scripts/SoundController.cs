using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundController {
	private static SoundController m_Instance;

	private AudioSource m_SeAudioSource;
	private AudioSource m_BgmAudioSource;

	private Dictionary<SOUND, List<AudioClip>> audioClips = new Dictionary<SOUND, List<AudioClip>>();


	public enum SOUND {
        BGM_TITLE,
        BGM_GAME_MAIN,
        BGM_GAME_OVER,

        SE_CLEAR,
        SE_PILLOW,
        SE_SHIFT,
        SE_BAD_end,
        SE_MostBAD_end,
        SE_TRUE_clear,
        SE_ANOTHOR_clear,
        SE_DEADKOICHI_end,
    };

	private SoundController()
	{
		addAudio (SOUND.BGM_TITLE, "Sounds/bgm_start");
        addAudio (SOUND.BGM_GAME_MAIN, "Sounds/bgm_play");
        addAudio (SOUND.SE_CLEAR, "Sounds/se_clear");
        addAudio (SOUND.BGM_GAME_OVER, "Sounds/se_gameover");
        addAudio (SOUND.SE_PILLOW, "Sounds/se_get");
        addAudio (SOUND.SE_BAD_end, "Sounds/se_bomb");
        addAudio (SOUND.SE_SHIFT, "Sounds/se_shift");
        addAudio(SOUND.SE_MostBAD_end, "Sounds/voice_gameclear_0");
        addAudio(SOUND.SE_TRUE_clear, "Sounds/voice_gameclear_1");
        addAudio(SOUND.SE_ANOTHOR_clear, "Sounds/voice_gameclear_2");
        addAudio(SOUND.SE_DEADKOICHI_end, "Sounds/voice_gameover");
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
        m_BgmAudioSource.volume = 0.6f;
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

