using UnityEngine;
using System.Collections.Generic;

public class AppleController : MonoBehaviour
{
    public GameObject applePrefab;
    public Player player; // プレイヤーオブジェクトをInspectorでアタッチするか、Findで取得

    public float spawnAvoidDistance = 0.5f; // スネークからの最小距離

    void Start()
    {
        Apple_Initialize();
    }

    public void Apple_Initialize()
    {
        Vector3 spawnPosition;
        
        int maxTries = 100; // 無限ループ防止

        // ビューポート（0〜1）→ ワールド座標への変換
        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0.05f, 0.05f, 0));
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(0.95f, 0.95f, 0));

        for (int i = 0; i < maxTries; i++)
        {
            float x = Random.Range(min.x, max.x);
            float y = Random.Range(min.y, max.y);
            spawnPosition = new Vector3(x, y, 0);

            if (IsSafePosition(spawnPosition))
            {
                Instantiate(applePrefab, spawnPosition, Quaternion.identity);
                return;
            }
        }

        Debug.LogWarning("リンゴの安全なスポーン位置が見つかりませんでした");
    }

    private bool IsSafePosition(Vector3 pos)
    {
        // プレイヤーが未設定の場合は安全とみなす
        if (player == null) return true;

        // 頭 + body 全体と比較
        if (Vector3.Distance(player.transform.position, pos) < spawnAvoidDistance)
            return false;

        foreach (Transform bodyPart in player.bodyParts)
        {
            if (Vector3.Distance(bodyPart.position, pos) < spawnAvoidDistance)
                return false;
        }
        return true;
    }
}
