using System;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649


// TODO
namespace Caxapexac.Common.Sharp.Runtime.Gui.Widgets
{
    public class FriendsPopup : MonoBehaviour
    {
        private int _itemHeight = 100;
        private const int Spacing = 8;
        private const int Top = 16;
        private const int Bottom = 50;

        [SerializeField]
        private FriendItemView[] Views;

        [SerializeField]
        private RectTransform Content;

        public event Action<int, FriendItemView> ItemShowed = delegate { };

        private int Count { get; set; }

        private float _y;
        private int _oldInd = -1;
        private RectTransform _item;

        private void Update()
        {
            _y = Content.anchoredPosition.y - Spacing;

            if (_y < 0) return;

            var inx = Mathf.FloorToInt(_y / (_itemHeight + Spacing));

            if (_oldInd == inx) return;

            //added to end
            if (inx > _oldInd)
            {
                var newInd = inx % Views.Length;

                newInd--;

                if (newInd < 0) newInd = Views.Length - 1;

                var id = inx + Views.Length - 1;

                if (id < Count)
                {
                    _item = Views[newInd].GetComponent<RectTransform>();

                    var pos = _item.anchoredPosition;

                    pos.y = -(Top + id * Spacing + id * _itemHeight);

                    _item.anchoredPosition = pos;

                    ItemShowed(id, Views[newInd]);
                }
            }

            //added to begin
            else
            {
                var newInd = inx % Views.Length;

                _item = Views[newInd].GetComponent<RectTransform>();

                var pos = _item.anchoredPosition;

                pos.y = -(Top + inx * Spacing + inx * _itemHeight);

                _item.anchoredPosition = pos;

                ItemShowed(inx, Views[newInd]);
            }

            _oldInd = inx;
        }

        public void SetData(int count)
        {
            _oldInd = 0;

            Count = count;

            var h = _itemHeight * count * 1f + Top + Bottom + (count == 0 ? 0 : ((count - 1) * Spacing));

            Content.sizeDelta = new Vector2(Content.sizeDelta.x, h);

            var pos = Content.anchoredPosition;
            pos.y = 0;
            Content.anchoredPosition = pos;

            var y = Top;

            for (int i = 0; i < Views.Length; i++)
            {
                var showed = i < count;

                Views[i].gameObject.SetActive(showed);

                if (showed)
                {
                    pos = Views[i].GetComponent<RectTransform>().anchoredPosition;
                    pos.y = -y;
                    Views[i].GetComponent<RectTransform>().anchoredPosition = pos;

                    y += Spacing + _itemHeight;

                    ItemShowed(i, Views[i]);
                }
            }
        }
    }


    public class FriendItemView : MonoBehaviour
    {
        [SerializeField]
        private Text NameText;

        public void SetData(string friendName)
        {
            NameText.text = friendName;
        }
    }


    public class Controller : MonoBehaviour
    {
        [SerializeField]
        private FriendsPopup Popup;

        // Use this for initialization
        private void Start()
        {
            Popup.ItemShowed += (index, view) => { view.SetData("item " + index); };

            Popup.SetData(1000);
        }
    }
}