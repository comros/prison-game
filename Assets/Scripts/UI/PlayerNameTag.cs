using TMPro;
using UnityEngine;

namespace sknco.prisongame
{
    public class PlayerNameTag : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;

        public void SetName(string name)
        {
            nameText.text = name;
        }
    }
}
