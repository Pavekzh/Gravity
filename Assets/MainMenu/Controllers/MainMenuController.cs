using System.Collections;
using UIExtended;
using BasicTools;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;

namespace Assets.Menu.Controllers
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] TMPro.TMP_Text timerText;
        [SerializeField] private int jorneySceneIndex;
        [SerializeField] private int sandboxSceneIndex;
        [SerializeField] private SettingsController settings;
        [SerializeField] private LevelLoader levelLoader;
        [SerializeField] private RateProposalController rateProposal;
        [SerializeField] private StateChanger menuOpener;

        private void Awake()
        {
            int MenuEntries = PlayerPrefs.GetInt("MenuEntries",0);
            PlayerPrefs.SetInt("MenuEntries", MenuEntries + 1);
            if (MenuEntries == 6)
                rateProposal.Open();
        }

        private void Start()
        {
            menuOpener.State = State.Changed;    
        }

        public void LoadPuzzle() 
        {
            levelLoader.LoadLevel(jorneySceneIndex);
        }

        public void LoadSandbox() 
        {
            levelLoader.LoadLevel(sandboxSceneIndex);
            StartCoroutine(timer());
        }

        public void Settings() 
        {
            settings.ChangeState();
        }

        public void Quit() 
        {
            Application.Quit();
        }

        float tStart;
        IEnumerator timer()
        {            
            tStart = Time.realtimeSinceStartup;
            while (true)
            {
                timerText.text = (Time.realtimeSinceStartup - tStart).ToString();
                yield return null;
            }
        }


    }
}