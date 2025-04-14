using TMPro;
using UnityEngine;

public class TextPunchEffect : MonoBehaviour
{
    [SerializeField] private TMP_Text _textMeshPro;
    [SerializeField] private float _force = 10f; // 吹き飛ばしの強さ
    [SerializeField] private float _gravity = 9.8f; // 重力

    // 各文字の状態保持用
    private Vector3[] _velocities; 
    private TMP_TextInfo _textInfo;

    void Start()
    {
        // TMP_TextInfoを取得して、文字数に合わせて配列準備
        _textInfo = _textMeshPro.textInfo;
        _velocities = new Vector3[_textInfo.characterCount];
    }

    public void Punch()
    {
        _textMeshPro.ForceMeshUpdate();
        _textInfo = _textMeshPro.textInfo;
        int characterCount = _textInfo.characterCount;

        // 各文字へランダム方向の初速を設定
        for (int i = 0; i < characterCount; i++)
        {
            if(!_textInfo.characterInfo[i].isVisible) continue;

            _velocities[i] = Random.insideUnitSphere * 5f; // 適当に5fくらい吹き飛ばす
        }
    }


    void Update()
    {
        // 1) メッシュ更新
        _textMeshPro.ForceMeshUpdate();
        _textInfo = _textMeshPro.textInfo;

        // 2) 文字数が変わっていたら再設定
        int characterCount = _textInfo.characterCount;
        if (_velocities == null || _velocities.Length != characterCount)
        {
            _velocities = new Vector3[characterCount];
        }

        // 3) 各文字に対して処理
        for (int i = 0; i < characterCount; i++)
        {
            // 文字が可視（描画可能）でなければスキップ
            if (!_textInfo.characterInfo[i].isVisible) 
                continue;

            int matIndex = _textInfo.characterInfo[i].materialReferenceIndex;
            if (matIndex < 0 || matIndex >= _textInfo.meshInfo.Length) 
                continue;

            int vertexIndex = _textInfo.characterInfo[i].vertexIndex;
            Vector3[] vertices = _textInfo.meshInfo[matIndex].vertices;

            // 頂点数が足りなければスキップ (安全策)
            if (vertexIndex + 3 >= vertices.Length) 
                continue;

            // ここで _velocities[i] を使って頂点移動などの処理
            // ...
        }

        // メッシュを再適用
        for (int i = 0; i < _textInfo.meshInfo.Length; i++)
        {
            var meshInfo = _textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            _textMeshPro.UpdateGeometry(meshInfo.mesh, i);
        }
    }

}