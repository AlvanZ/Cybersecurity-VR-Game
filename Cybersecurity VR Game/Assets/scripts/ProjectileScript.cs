using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
public int answerindex;
    private void OnCollisionEnter(Collision collision)
    {
        print("hit");

        // Inform the survey manager about the chosen answer
        SurveyManagerBase.Instance.AnswerChosen(answerindex);

        // Destroy the projectile
        
        return;
    }
}
