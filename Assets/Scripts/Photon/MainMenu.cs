using System;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Photon
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private InputField enterRoomName;
        [SerializeField] private Button hostButton;
        [SerializeField] private Button clientButton;
        
        private NetworkRunner _runner;

        private void Start()
        {
            hostButton.onClick.AddListener(() => StartGame(GameMode.Host));
            clientButton.onClick.AddListener(() => StartGame(GameMode.Client));
            DontDestroyOnLoad(gameObject);
        }

        private async void StartGame(GameMode mode)
        {
            var roomName = enterRoomName.text;
            if (string.IsNullOrEmpty(roomName)) roomName = "TestRoom";

            _runner = gameObject.AddComponent<NetworkRunner>();
            _runner.ProvideInput = true;

            await _runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = roomName,
                Scene = 1,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }
    }
}