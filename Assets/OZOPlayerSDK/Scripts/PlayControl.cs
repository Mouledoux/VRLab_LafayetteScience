﻿using UnityEngine;
using System.Collections;
using OZO;

public class PlayControl : MonoBehaviour {

    [SerializeField]
    private OZOPlayer   _playerView = null;
    [SerializeField]
    private GameObject  _loadingIndicator = null;
    [SerializeField]
    private string _licenseId = "INSERT_YOUR_OZO_LICENSE_ID_HERE";
    [SerializeField]
    private string[]    _videoFiles = null;

    private int _videoIndex = -1;

#if UNITY_EDITOR
    private bool _editorIsPaused = false;
#endif

    private IOZOPlayer _play = null;
    public bool _startPlay = false;

    private Quaternion _rotationOffset = Quaternion.identity;

    void Awake()
    {
        if (_playerView != null)
        {
            _play = _playerView;
            _play.Init(_licenseId, true, false, false); //the "allow exclusive mode audio" works only in standalone player, when you have set "Disable Unity Audio"
        }
    }
    
    // Use this for initialization
    void Start ()
    {
        UnityEngine.VR.InputTracking.Recenter();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.playmodeStateChanged = HandleOnPlayModeChanged;
#endif

        if (_playerView != null)
        {
            _play = _playerView;
            _play.OnPlayModeChanged += MPlay_OnPlayModeChanged;
        }
        else
        {
            Debug.LogError("Please Set the OZOPlayer for PlayControl");
        }
    }

    //These events can be used for controlling the other application, for example UI, etc
    private void MPlay_OnPlayModeChanged(VideoPlaybackState state)
    {
        Debug.Log("VideoPlaybackState: " + state.ToString());

        if(state == VideoPlaybackState.END_OF_FILE)
        {
            _play.SeekTo(0);
            _play.PlayLoaded();
        }
        if(_loadingIndicator )
        {
            if(state == VideoPlaybackState.PLAYING )
            {
                _loadingIndicator.SetActive(false);
            }
            else
            {
                _loadingIndicator.SetActive(true);
            }
        }
    }

    void OnDestroy()
    {
        if (null != _play)
            _play.OnPlayModeChanged -= MPlay_OnPlayModeChanged;
    }
    void OnApplicationQuit()
    {
        if(null != _play)
            _play.OnPlayModeChanged -= MPlay_OnPlayModeChanged;
    }

#if UNITY_EDITOR
    void HandleOnPlayModeChanged()
    {
        if (null != _play)
        {
            //mainly to pause the custom audio rendering
            if (UnityEditor.EditorApplication.isPaused && !_editorIsPaused)
            {
                _editorIsPaused = !_editorIsPaused;

                if (_editorIsPaused)
                    _play.Pause();
                else
                    _play.Resume();
            }
            else if(!UnityEditor.EditorApplication.isPaused && _editorIsPaused)
            {
                _editorIsPaused = !_editorIsPaused;
                if (_editorIsPaused)
                    _play.Pause();
                else
                    _play.Resume();
            }
        }
    }
#endif

    // Update is called once per frame
    void Update ()
    {
        if (null==_play)
            return;

        if (0 == _videoFiles.Length)
            return;



        #region OLD_CODE
//        if (Input.GetKeyDown(KeyCode.Escape))
                         //        {
                         //#if UNITY_EDITOR
                         //            UnityEditor.EditorApplication.isPlaying = false;
                         //#else
                         //            Application.Quit();
                         //#endif
                         //        }
                         //#if (UNITY_EDITOR_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE || UNITY_STANDALONE_OSX)
                         //        //controls
                         //        else if (Input.GetKey(KeyCode.LeftArrow))
                         //        {
                         //            _rotationOffset *= Quaternion.Euler(0.0f, -1.0f, 0.0f);
                         //            _play.SetViewRotationOffset(_rotationOffset);
                         //        }
                         //        else if (Input.GetKey(KeyCode.RightArrow))
                         //        {
                         //            _rotationOffset *= Quaternion.Euler(0.0f, 1.0f, 0.0f);
                         //            _play.SetViewRotationOffset(_rotationOffset);
                         //        }
                         //        else if (Input.GetKey(KeyCode.UpArrow))
                         //        {
                         //            _rotationOffset *= Quaternion.Euler(-1.0f, 0.0f, 0.0f);
                         //            _play.SetViewRotationOffset(_rotationOffset);
                         //        }
                         //        else if (Input.GetKey(KeyCode.DownArrow))
                         //        {
                         //            _rotationOffset *= Quaternion.Euler(1.0f, 0.0f, 0.0f);
                         //            _play.SetViewRotationOffset(_rotationOffset);
                         //        }
                         //        else if (Input.GetKey(KeyCode.Return))
                         //        {
                         //            _rotationOffset = Quaternion.identity;
                         //            _play.SetViewRotationOffset(_rotationOffset);
                         //        }
                         //#else
                         //        _play.SetViewRotationOffset(_rotationOffset); //this could be set with some input values
                         //#endif

        //        bool pressed = Input.GetButtonDown("Fire1");

        //        VideoPlaybackState state = VideoPlaybackState.INVALID;
        //        //simple input controls
        //        if (_startPlay && null != _play && Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || pressed)
        //        {
        //            state = _play.GetCurrentVideoPlaybackState();
        //            if (state == VideoPlaybackState.PLAYING)
        //            {
        //                _play.Stop();
        //            }
        //            _startPlay = true;
        //        }

        //        //begin the playback when the player is in good state (ie. previous video has been stopped)
        //        if(_startPlay)
        //        {
        //            state = _play.GetCurrentVideoPlaybackState();
        //            if ((VideoPlaybackState.IDLE == state)) 
        //            {
        //                _startPlay = false;
        //                _videoIndex = (_videoIndex + 1) % _videoFiles.Length;

        //                if (!_play.LoadVideo (_videoFiles [_videoIndex])) 
        //                {
        //                    Debug.LogError("Error: Could not load! (" + _play.GetLastError ().ToString () + ")");
        //                    return;
        //                }
        //                //start playing when the file/stream is ready
        //                if (!_play.PlayLoaded ()) 
        //                {
        //                    Debug.LogError("Error: Could not start the playback! (" + _play.GetLastError ().ToString () + ")");
        //                    return;
        //                }
        //            }
        //        }
#endregion
    }

    public void PlayVideoAtIndex(int index)
    {
        _play.LoadVideo(_videoFiles[index]);
        _play.PlayLoaded();
    }
}
