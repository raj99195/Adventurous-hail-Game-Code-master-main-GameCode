using UnityEngine;

public class LevelDesigner : MonoBehaviour
{
    public static LevelDesigner Instance;
    public Step[] steps;

    //number step for put arrays
    [HideInInspector] public int stepNumber;
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }

        prepareStepsData();

        SupriseManeger.Instance.CanRunSurpriseStep = new bool[steps.Length];
    }


    /// <summary>
    /// Checks steps data to make sure it is correct
    /// </summary>
    void prepareStepsData()
    {
        if (steps.Length == 0)
        {
            emergencyExit("You need to create at least one step to run the game. (Level Designer GameObject >> Inspector window >> Array steps)");
        }

        string stepName = "", nextStepName = "";
        for (int l = 0; l < steps.Length; l++)
        {
            stepName = steps[l].name.Trim();
            if (stepName == "")
            {
                stepName = "Element " + l;
            }
            if (steps[l].maxScoreStep <= 0)
            {
                emergencyExit("The value of maxScoreStep should not be 0 or less than 0 in step: < " + stepName + " > ");
            }

            if ((steps.Length > 1) && (l < steps.Length - 1))
            {

                nextStepName = steps[l + 1].name.Trim();
                if (nextStepName == "")
                {
                    nextStepName = "Element " + (l + 1);
                }

                if (steps[l].maxScoreStep >= steps[l + 1].maxScoreStep)
                {
                    emergencyExit("The maxScoreStep value in Step: < " + stepName + " > should not be equal to or greater than the maxScoreStep in Step: < " + nextStepName + " >");
                }
            }

            if (steps[l].delayOfCreateHail == 0)
            {
                emergencyExit("The delayOfCreateHail value in step: < " + stepName + " >  should not be equal to 0.");
            }

            if (steps[l].SpeedHail == 0)
            {
                emergencyExit("The SpeedHail value in step: < " + stepName + " >  should not be equal to 0.");
            }

            if ((steps[l].slowMotion || steps[l].chance) && steps[l].LifeTimeOfSlowMotionSuprise == 0)
            {
                emergencyExit("The LifeTimeOfSlowMotionSuprise value in step: < " + stepName + " >  should not be equal to 0.");
            }

            if ((steps[l].changeHailSpeed || steps[l].chance) && steps[l].LifeTimeOfSpeedSuprise == 0)
            {
                emergencyExit("The LifeTimeOfSpeedSuprise value in step: < " + stepName + " >  should not be equal to 0.");
            }

            steps[l].activeSupriseList = new bool[7];
            steps[l].activeSupriseList[0] = steps[l].changeHailColor;
            steps[l].activeSupriseList[1] = steps[l].changeColorPalette;
            steps[l].activeSupriseList[2] = steps[l].changeHailSpeed;
            steps[l].activeSupriseList[3] = steps[l].slowMotion;
            steps[l].activeSupriseList[4] = steps[l].changeDirctionHail;
            steps[l].activeSupriseList[5] = steps[l].destroyHails;
            steps[l].activeSupriseList[6] = steps[l].chance;


            foreach (var item in steps[l].activeSupriseList)
            {
                if (item)
                {
                    if (steps[l].delayOfCreateSurprises == 0)
                    {
                        emergencyExit("The delayOfCreateSurprises value in step: < " + stepName + " >  should not be equal to 0.");
                        break;
                    }
                    else if (steps[l].theMinimumDistanceToCreateSurpriseFromTop > steps[l].theMaximumDistanceToCreateSurpriseFromTop)
                    {
                        emergencyExit("In step: < " + stepName + " >, the value of theMinimumDistanceToCreateSurpriseFromTop should not be greater than the value of theMaximumDistanceToCreateSurpriseFromTop");
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    ///This function calculates and applies the game stage changes according to the value given to it according to the game score given to it.
    /// </summary>
    /// <param name="GoToLevel(int levelNumber)"></param>
    public void GoToLevel(int levelNumber)
    {
        for (int i = 0; i < steps.Length; i++)
        {
            if (levelNumber <= steps[0].maxScoreStep)
            {
                Debug.Log("Step = " + (i));
                stepNumber = 0;
                SupriseManeger.Instance.canCreateSurprise = true;
                SupriseManeger.Instance.CanRunSurpriseStep[stepNumber] = true;
                break;
            }
            else if (i < steps.Length - 1)
            {
                if (levelNumber > steps[i].maxScoreStep && levelNumber <= steps[(i + 1)].maxScoreStep)
                {
                    Debug.Log("Step = " + (i + 1));
                    SupriseManeger.Instance.CanRunSurpriseStep[i] = false;
                    setsBoolianToChangeTheSpeedofHailsThatAreStillInTheGameplay(levelNumber, i);
                    stepNumber = (i + 1);
                    SupriseManeger.Instance.CanRunSurpriseStep[stepNumber] = true;
                    break;
                }
            }
            else if (i == steps.Length - 1)
            {
                SupriseManeger.Instance.CanRunSurpriseStep[i] = false;
                setsBoolianToChangeTheSpeedofHailsThatAreStillInTheGameplay(levelNumber, i);
                stepNumber = (steps.Length - 1);
                SupriseManeger.Instance.CanRunSurpriseStep[stepNumber] = true;
                break;
            }
        }
    }
    void setsBoolianToChangeTheSpeedofHailsThatAreStillInTheGameplay(int receiveScoreForCompare, int numberLastStep)
    {

        if (receiveScoreForCompare >= steps[numberLastStep].maxScoreStep)
        {
            HailCreater.Instance.canSetTheSpeedOfTheHailsThatAreStillInTheGameplayWithTheSpeedOfTheNewStep = true;

        }
    }

    /// <summary>
    /// Exit the game due to incomplete input information in >> Level Designer GameObject >> Inspector window >> Array steps
    /// </summary>
    void emergencyExit(string message)
    {
        Debug.LogError(message);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }

    /// <summary>
    /// This function is called when the level designer is to be reset to the first step.
    /// </summary>
    /// <param name="ResetToFirstLevel()"></param>
    public void ResetToFirstLevel()
    {
        stepNumber = 0;
        SupriseManeger.Instance.CanRunSurpriseStep[stepNumber] = true;
        SupriseManeger.Instance.SupriseRestartForLevelDesigner();
        HailCreater.Instance.HailCreaterRestart();
        for (int i = 1; i <= steps.Length - 1; i++)
        {
            SupriseManeger.Instance.CanRunSurpriseStep[i] = false;
        }
    }

    [System.Serializable]
    /// <summary>
    /// Each step identifies features for a number of levels
    /// </summary>
    public class Step
    {
        public string name;
        public int maxScoreStep;

        [Range(0.3f, 4)]
        public float delayOfCreateHail;

        [Range(2, 10)]
        public float SpeedHail;

        [Header("Suprises")]
        public bool changeHailColor;

        [Space]
        public bool changeColorPalette;

        [Space]
        public bool changeHailSpeed;
        [Range(.5f, 3)]
        public float LifeTimeOfSpeedSuprise;

        [Space]
        public bool slowMotion;
        [Range(.5f, 3)]
        public float LifeTimeOfSlowMotionSuprise;

        [Space]
        public bool changeDirctionHail;

        [Space]
        public bool destroyHails;

        [Space]
        public bool chance;

        [Space]
        [Range(0.2f, 20f)]
        public float delayOfCreateSurprises;

        [Range(0, 12)]
        public int theMinimumDistanceToCreateSurpriseFromTop;

        [Range(0, 12)]
        public int theMaximumDistanceToCreateSurpriseFromTop;

        [HideInInspector]
        public bool[] activeSupriseList;
    }
}