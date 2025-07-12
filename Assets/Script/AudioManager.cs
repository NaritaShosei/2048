using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [SerializeField] List<AudioData> _datas;
    [SerializeField] AudioSource _source;

    Dictionary<SceneType, AudioClip> _bgmDic = new Dictionary<SceneType, AudioClip>();

    private void Awake()
    {
        foreach (var data in _datas)
        {
            if (!_bgmDic.ContainsKey(data.KeyType))
            {
                _bgmDic.Add(data.KeyType, data.AudioClip);
            }
        }
    }
    public void PlayBGM(SceneType type)
    {
        if (_bgmDic.TryGetValue(type, out AudioClip clip))
        {
            _source.clip = clip;
            _source.loop = true;
            _source.Play();
        }
    }
}
[System.Serializable]
public class AudioData
{
    [SerializeField] SceneType _keyType;
    [SerializeField] AudioClip _audioClip;
    public SceneType KeyType => _keyType;
    public AudioClip AudioClip => _audioClip;

}