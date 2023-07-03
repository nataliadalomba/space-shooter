```cs
using System;
using UnityEngine;

namespace MarksAdventure {
    public class SpawnControllerExample : MonoBehaviour {
        [Serializable]
        private struct SpawnInfo {
            public GameObject prefab;

            [Range(0, 1)]
            public float percentChance; //NOTE: You might use a different scheme for determining how often your power-ups spawn.
        }

        [SerializeField] private SpawnInfo[] allSpawns = { };

        //TODO: Implementation...
    }
}
```

![Spawn Controller Example 1](./docs/images/Spawn%Controller%Example%1.png)

```cs
using System;
using UnityEngine;

namespace MarksAdventure {
    public class SpawnControllerExample : MonoBehaviour {
        [Serializable]
        private struct SpawnInfo {
            public GameObject prefab;

            public float minSpawnTime;
            public float maxSpawnTime;
        }

        [SerializeField] private SpawnInfo[] allSpawns = {
            new SpawnInfo {
                minSpawnTime = 10,
                maxSpawnTime = 12
            }
        };

        //TODO: Implementation...
    }
}
```

![Spawn Controller Example 2](./docs/images/Spawn%Controller%Example%2.png)

Implement the above example to have the Wave PowerUp spawn more rarely than the existing power ups. This is the last of the Wave PowerUp feature.

<br />

Create final animated sprite for Wave PowerUp Collectable

<br />

Finish Wave PowerUp article.