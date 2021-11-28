using System.Collections.Generic;
using UnityEngine;

public class PhotoAbleEntityController {
    private static PhotoAbleEntityController instance = null;
    private PhotoAbleEntityController() {
        CanPotoGODict = new Dictionary<GameObject, TileDatas>();
    }
    public static PhotoAbleEntityController GetInstance {
        get {
            if (instance == null) {
                instance = new PhotoAbleEntityController();
            }
            return instance;
        }
    }

    public Dictionary<GameObject, TileDatas> CanPotoGODict;
}
