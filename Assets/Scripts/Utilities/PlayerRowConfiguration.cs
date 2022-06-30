using Sfs2X.Entities;
using UnityEngine;

public class PlayerRowConfiguration : MonoBehaviour
{
    [Header("Setup variable")]
    [SerializeField] private TMPro.TMP_Text title;

    public User user;

    //Methods:
    public void Initialise(User user)
    {
        this.title.text = user.Name;
        this.user = user;
    }
}
