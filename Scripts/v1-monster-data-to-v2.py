from collections import OrderedDict
import requests
import xmltodict
import logging
from typing import Dict, List
import os

FORMAT = '%(asctime)s [%(levelname)s] %(message)s'
logging.basicConfig(format=FORMAT, level=logging.INFO)
logger = logging.getLogger("monster_data_converter")

remote_v1_monster_data = "https://raw.githubusercontent.com/Haato3o/HunterPie/master/HunterPie/HunterPie.Resources/Data/MonsterData.xml" 

def load_v1_data() -> Dict:
    logger.info("Requesting remote v1 data")
    req = requests.get(remote_v1_monster_data)

    logger.info("Parsing XML into Dictionary")
    return xmltodict.parse(req.text)

def save_v2_data(data: OrderedDict):
    if not os.path.exists("output"):
        os.mkdir("output")

    with open("output/out_monster_data_v2.xml", "w") as output:
        xmltodict.unparse(data, output, 'UTF-8', pretty = True)

def extract_crown(monster) -> OrderedDict:
    crowns = OrderedDict()

    crowns["@Mini"] = monster["Crown"]["@Mini"]
    crowns["@Silver"] = monster["Crown"]["@Silver"]
    crowns["@Gold"] = monster["Crown"]["@Gold"]
    
    return crowns

def extract_parts(monster) -> List[OrderedDict]:
    parts = OrderedDict(Part = [])

    id = 0
    for part in monster["Parts"]["Part"]:
        data = OrderedDict()

        data["@Id"] = id if part.get("@Index") == None else part["@Index"]
        data["@String"] = part["@Name"]

        if (part.get("@TenderizeIds") != None):
            data["@TenderizeIds"] = part["@TenderizeIds"]

        parts["Part"].append(data)
        id += 1

    return parts

def iterate_monsters(monsters: OrderedDict) -> OrderedDict:
    monster_list = []

    for monster in monsters:
        node = OrderedDict()
        node["@Id"] = monster["@GameID"]
        node["Crowns"] = extract_crown(monster)
        node["Parts"] = extract_parts(monster)
        monster_list.append(node)

    logger.info(f"Converted {len(monsters)} monsters")

    monster_list = sorted(monster_list, key = lambda x: int(x['@Id']))

    return monster_list

def iterate_ailments(ailments: OrderedDict) -> OrderedDict:
    ailments_list = []

    for ailment in ailments:
        node = OrderedDict()
        node["@Id"] = ailment["@Id"]
        node["@String"] = ailment["@Name"].replace("STATUS_", "AILMENT_")
        ailments_list.append(node)

    return ailments_list


def generate_v2_data(ailments: List[OrderedDict], monsters: List[OrderedDict]) -> OrderedDict:
    ailmentsNode = OrderedDict(Ailment = ailments)
    monstersNode = OrderedDict(Monsters = monsters)

    root = OrderedDict(GameData = OrderedDict(
        Ailments = ailmentsNode,
        Monsters = monstersNode
    ))
    return root

def main():
    v1_data = load_v1_data()

    v2_monsters = iterate_monsters(v1_data["Monsters"]["Monster"])
    v2_ailments = iterate_ailments(v1_data["Monsters"]["Ailments"]["Ailment"])
    v2_data = generate_v2_data(v2_ailments, v2_monsters)

    save_v2_data(v2_data)


if __name__ == "__main__":
    
    logger.info("Starting v1 monster data conversion")
    main()
    logger.info("Finished converting v1 monster data to v2")