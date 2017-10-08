using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SubMonster
{
    [XmlAttribute("name")]
    public string Name;
}

public class Monster
{
    [XmlAttribute("name")]
    public string Name;
    public int Health;
    public string AttackType;
    [XmlArray("SubMonsters"), XmlArrayItem("SubMonster")]
    public List<SubMonster> SubMonsters;
}


[XmlRoot("MonsterCollection")]
public class MonsterContainer
{
    [XmlArray("Monsters"), XmlArrayItem("Monster")]
    public List<Monster> Monsters;


    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(MonsterContainer));
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static MonsterContainer Load(string path)
    {
        var serializer = new XmlSerializer(typeof(MonsterContainer));
        using (FileStream stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as MonsterContainer;
        }
    }


    public static MonsterContainer LoadFromText(string text)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(MonsterContainer));
        return serializer.Deserialize(new StringReader(text)) as MonsterContainer;
    }
}

/** Inside, monsters.xml, located in Assets folder
<MonsterCollection>
 	<Monsters>
 		<Monster name="a">
 			<Health>5</Health>
			<AttackType>"Normal"</AttackType>
			<SubMonsters>
				<SubMonster name="ab">
				</SubMonster>
			</SubMonsters>
 		</Monster>
 		<Monster name="b">
 			<Health>3</Health>
			<AttackType>"Magic"</AttackType>
			<SubMonsters>
				<SubMonster name="abc">
				</SubMonster>
			</SubMonsters>
 		</Monster>
 	</Monsters>
 </MonsterCollection>
**/





public class TestXML : MonoBehaviour{

    MonsterContainer monsterCollection;
    public string monsterName;
    public int monsterHealth;
    public string attackType;
    public List<Monster> monsterList;

    /*********************/

    void Start()
    {
        monsterCollection = MonsterContainer.Load(Path.Combine(Application.dataPath, "monsters.xml"));
        monsterList = monsterCollection.Monsters;
        monsterName = monsterList[1].Name;
        monsterHealth = monsterList[1].Health;
        attackType = monsterList[1].AttackType;

        Monster monster= new Monster();
        monster.Name = monsterCollection.Monsters[1].SubMonsters[0].Name;


    }
}
