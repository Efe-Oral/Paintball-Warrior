using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialController : MonoBehaviour
{
    private struct Lesson
    {
        public string Title;
        public string Description;
        public string[] Bullets;

        public Lesson(string title, string description, string[] bullets)
        {
            Title = title;
            Description = description;
            Bullets = bullets;
        }
    }

    [Header("Gameplay to freeze while a lesson is showing")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PaintSprayer paintSprayer;
    [SerializeField] private LevelTimer timer;

    [Header("Panel")]
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private TMP_Text badgeText;
    [SerializeField] private Image[] progressDots;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text[] bulletTexts;
    [SerializeField] private Button letsGoButton;
    [SerializeField] private Button skipButton;

    [Header("Dot colors")]
    [SerializeField] private Color activeDotColor = new Color(0.95f, 0.78f, 0.15f, 1f);
    [SerializeField] private Color inactiveDotColor = new Color(0.85f, 0.85f, 0.85f, 1f);

    private Lesson[] lessons;
    private int currentLesson;

    private void Awake()
    {
        lessons = new[]
        {
            new Lesson(
                "Move & Spray",
                "Your gun auto-fires paint while you move. Cover the floor before your tank or the clock runs out.",
                new[] { "Move with WASD or the joystick", "Auto-fire while moving", "Watch your paint tank", "Cover the floor" }),
            new Lesson(
                "Paint the Structures",
                "Crates and props need paint on every side to count — walking around them is the only way.",
                new[] { "Walk around structures to hit every angle", "They turn green once fully painted", "Structures count toward 100%", "No partial credit" }),
            new Lesson(
                "Beat the Clock",
                "Reach 100% coverage before your tank runs dry or the timer hits zero — whichever comes first ends the level.",
                new[] { "Watch the timer in the top bar", "Watch your paint tank", "Finish before either runs out", "Both must survive to the end" }),
        };

        playerMovement.enabled = false;
        paintSprayer.enabled = false;
        timer.enabled = false;

        letsGoButton.onClick.AddListener(LetsGo);
        skipButton.onClick.AddListener(SkipTutorial);

        currentLesson = 0;
        ShowLesson(0);
        panelRoot.SetActive(true);
    }

    private void LetsGo()
    {
        currentLesson++;
        if (currentLesson >= lessons.Length)
        {
            Dismiss();
            return;
        }

        ShowLesson(currentLesson);
    }

    private void SkipTutorial()
    {
        Dismiss();
    }

    private void ShowLesson(int index)
    {
        Lesson lesson = lessons[index];

        badgeText.text = "LESSON " + (index + 1) + " OF " + lessons.Length;
        titleText.text = lesson.Title;
        descriptionText.text = lesson.Description;

        for (int i = 0; i < bulletTexts.Length; i++)
        {
            bulletTexts[i].text = i < lesson.Bullets.Length ? lesson.Bullets[i] : string.Empty;
        }

        for (int i = 0; i < progressDots.Length; i++)
        {
            progressDots[i].color = i == index ? activeDotColor : inactiveDotColor;
        }
    }

    private void Dismiss()
    {
        panelRoot.SetActive(false);

        playerMovement.enabled = true;
        paintSprayer.enabled = true;
        timer.enabled = true;
    }
}
