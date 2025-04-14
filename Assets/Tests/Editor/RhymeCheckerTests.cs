using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.TestTools;


public class RhymeCheckerTests
{
     RhymeChecker rhymeChecker;

    [SetUp]
    public void Setup()
    {
        // テスト用に RhymeChecker のインスタンスを新規生成
        // もし RhymeChecker が MonoBehaviour を継承しているなら
        // オブジェクトを作ってアタッチする形でやります
        var go = new GameObject("RhymeCheckerTestObject");
        rhymeChecker = go.AddComponent<RhymeChecker>();

        // 必要であれば RhymeData をアサイン
        // rhymeChecker.rhymeData = テスト用の ScriptableObject など
    }

    [TearDown]
    public void Teardown()
    {
        // テストが終わったらオブジェクトを破棄
        Object.DestroyImmediate(rhymeChecker.gameObject);
    }

    [Test]
    public void TestIsRhyme_BasicCase()
    {
        // 例えば「アキバ」 vs 「ガキダ」が韻を踏むかどうか
        bool result = rhymeChecker.IsRhyme("アキバ", "ガキダ", 2);
        Assert.IsTrue(result, "アキバ vs ガキダ は後ろの母音が一致するので韻判定がTrueになるはず");
    }

    [Test]
    public void TestIsRhyme_NoRhyme()
    {
        bool result = rhymeChecker.IsRhyme("アキバ", "ワサビ", 2);
        Assert.IsFalse(result, "アキバ vs ワサビ は母音が違うので韻判定がFalseになるはず");
    }

    // ... 他のテストパターンも追加
}