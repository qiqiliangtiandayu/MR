using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Data;
using Newtonsoft.Json;
using UnityEngine;

namespace Managers
{
    public class DataManager : Singleton<DataManager>
    {
        internal string DataPath;


        public Dictionary<int, ItemDefine> Items = null;
        public Dictionary<int, CharacterDefine> Characters = null;
        public Dictionary<int, StateDefine> States = null;
        public DataManager()
        {
            DataPath = Path.Combine(Application.dataPath, "Common/Tables/Data/");
        }

        internal void Load()
        {
            string json = File.ReadAllText(this.DataPath + "ItemDefine.txt");
            //this.Items = JsonConvert.DeserializeObject<Dictionary<int, ItemDefine>>(json);
            json = File.ReadAllText(this.DataPath + "CharacterDefine.txt");
            this.Characters = JsonConvert.DeserializeObject<Dictionary<int, CharacterDefine>>(json);
            json = File.ReadAllText(this.DataPath + "StateDefine.txt");
            this.States = JsonConvert.DeserializeObject<Dictionary<int, StateDefine>>(json);
        }

        public IEnumerator LoadData()
        {
            string json = File.ReadAllText(this.DataPath + "ItemDefine.txt");
            this.Items = JsonConvert.DeserializeObject<Dictionary<int, ItemDefine>>(json);
            yield return null;
            json = File.ReadAllText(this.DataPath + "CharacterDefine.txt");
            this.Characters = JsonConvert.DeserializeObject<Dictionary<int, CharacterDefine>>(json);
            yield return null;
            json = File.ReadAllText(this.DataPath + "StateDefine.txt");
            this.States = JsonConvert.DeserializeObject<Dictionary<int, StateDefine>>(json);
            yield return null;
        }
    }
}