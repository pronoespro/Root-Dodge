using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PronoesPro.Entity
{
    public class Resource : MonoBehaviour
    {

        public string resourceName;

        public int resource;
        public int maxResource;

        public UnityEvent OnResourceChanged;

        public virtual void Start()
        {
            OnResourceChanged.AddListener(UpdateAmmount);
        }

        private void OnDestroy()
        {
            OnResourceChanged.RemoveAllListeners();
        }

        public void ChangeResourceAmmount(string data)
        {
            string[] splitData = data.Split(',');
            if (splitData.Length > 1)
            {
                int ammount;
                if (splitData[0] == resourceName && int.TryParse(splitData[1], out ammount))
                {
                    resource = ammount;
                    OnResourceChanged.Invoke();
                }
            }
        }

        public void AddToResource(string data)
        {
            string[] splitData = data.Split(',');
            if (splitData.Length > 1)
            {
                int ammount;
                if (splitData[0] == resourceName && int.TryParse(splitData[1], out ammount))
                {
                    resource += ammount;
                    OnResourceChanged.Invoke();
                }
            }
        }

        public void RemoveFromResource(string data)
        {
            string[] splitData = data.Split(',');
            if (splitData.Length > 1)
            {
                int ammount;
                if (splitData[0] == resourceName && int.TryParse(splitData[1], out ammount))
                {
                    resource -= ammount;
                    OnResourceChanged.Invoke();
                }
            }
        }

        public void ChangeResourceAmmount(int ammount)
        {
            resource = ammount;
            OnResourceChanged.Invoke();
        }

        public void AddToResource(int ammount)
        {
            resource += ammount;
            OnResourceChanged.Invoke();
        }

        public void RemoveFromResource(int ammount)
        {
            resource -= ammount;
            OnResourceChanged.Invoke();
        }

        public bool NeedResource(string data)
        {
            string[] spitData = data.Split(',');
            if (spitData.Length > 1 && spitData[0] == resourceName)
            {
                int ammount;
                if (int.TryParse(spitData[1], out ammount))
                {
                    if (resource >= ammount)
                    {
                        SendMessage("HasResource");
                        OnResourceChanged.Invoke();
                        return true;
                    }
                }
            }
            return false;
        }

        public bool UseResource(string data)
        {
            string[] spitData = data.Split(',');
            if (spitData.Length > 1 && spitData[0] == resourceName)
            {
                int ammount;
                if (int.TryParse(spitData[1], out ammount))
                {
                    if (resource >= ammount)
                    {
                        SendMessage("HasResource");
                        resource -= ammount;
                        OnResourceChanged.Invoke();
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IfLessThanResource(string data)
        {
            string[] spitData = data.Split(',');
            if (spitData.Length > 1 && spitData[0] == resourceName)
            {
                int ammount;
                if (int.TryParse(spitData[1], out ammount))
                {
                    if (resource < ammount)
                    {
                        SendMessage("HasResource");
                        OnResourceChanged.Invoke();
                        return true;
                    }
                }
            }
            return false;
        }

        public void IfLostResource(string data)
        {
            string[] spitData = data.Split(',');
            if (spitData.Length > 1 && spitData[0] == resourceName)
            {
                int ammount;
                if (int.TryParse(spitData[1], out ammount))
                {
                    if (maxResource-resource >= ammount)
                    {
                        SendMessage("HasResource");
                        OnResourceChanged.Invoke();
                    }
                }
            }
        }

        public void UpdateAmmount()
        {
            if (maxResource > 0)
            {
                resource = Mathf.Clamp(resource, 0, maxResource);
            }
        }

    }
}