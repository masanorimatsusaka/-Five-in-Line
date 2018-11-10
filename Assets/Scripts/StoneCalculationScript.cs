using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StoneCalculationScript {
    public static int[,] bord=new int[19,19];
    /// <summary>
    /// ボードの初期化
    /// </summary>
    public static void ResetBord()
    {
        int[,] bord = new int[19, 19];
    }
    /// <summary>
    /// 置かれたストーンを補正して位置を返却する。
    /// </summary>
    public static bool SetStonePosition(GameObject stone,bool blackFlag)
    {
        Vector3 oldVector = stone.transform.position;
        Vector3 newVevtor;
        newVevtor.y = 1.5f;

        double x_min = 99.0;
        float result = 0.0f;
        int x_stance = 99;
        //x軸の近似値を格納
        for (int i = 0; i < 19; i++)
        {
            if (x_min > System.Math.Abs(oldVector.x-(-0.9+i*0.1)))
            {
              
                x_min = System.Math.Abs(oldVector.x - (-0.9 + i * 0.1));
                result = (-0.9f + i * 0.1f);
                x_stance = i;
            }
        }
        newVevtor.x = result;
        double z_min = 99.0;
        result = 0.0f;
        int z_stance = 99;
        //x軸の近似値を格納
        for (int i = 0; i < 19; i++)
        {
            if (z_min > System.Math.Abs(oldVector.z - (0.9 - i * 0.1)))
            {
                z_min = System.Math.Abs(oldVector.z - (0.9 - i * 0.1));
                result = (0.9f - i * 0.1f);
                z_stance = i;
            }
        }
        newVevtor.z = result;
        if (bord[x_stance, z_stance] != 0)
        {
            return false;
        }
        if (blackFlag)
        {
            bord[x_stance, z_stance] = 1;
        }
        else
        {
            bord[x_stance, z_stance] = 2;
        }
       
        ScanResult();
        if (endFlag)
        {
            return false;
        }
        stone.transform.position = newVevtor;
        stone.name = "Stone(Clone)";
        return true;
    }
    /// <summary>
    /// bordの同期メソッド
    /// </summary>
    public static void SerializeBord(Vector3 oldVector,bool blackFlag)
    {
        Vector3 newVevtor;
        newVevtor.y = 1.5f;

        double x_min = 99.0;
        float result = 0.0f;
        int x_stance = 99;
        //x軸の近似値を格納
        for (int i = 0; i < 19; i++)
        {
            if (x_min > System.Math.Abs(oldVector.x - (-0.9 + i * 0.1)))
            {

                x_min = System.Math.Abs(oldVector.x - (-0.9 + i * 0.1));
                result = (-0.9f + i * 0.1f);
                x_stance = i;
            }
        }
        newVevtor.x = result;
        double z_min = 99.0;
        result = 0.0f;
        int z_stance = 99;
        //x軸の近似値を格納
        for (int i = 0; i < 19; i++)
        {
            if (z_min > System.Math.Abs(oldVector.z - (0.9 - i * 0.1)))
            {
                z_min = System.Math.Abs(oldVector.z - (0.9 - i * 0.1));
                result = (0.9f - i * 0.1f);
                z_stance = i;
            }
        }
        newVevtor.z = result;
        if (blackFlag)
        {
            bord[x_stance, z_stance] = 1;
        }
        else
        {
            bord[x_stance, z_stance] = 2;
        }
        ScanResult();
    }

    public static string winnerText="";
    public static string blackScore="-";
    public static string whiteScore="-";
    public static bool endFlag = false;
    #region Bordの計算
    static readonly int[] BlackThreeLine = { 0, 1, 1, 1, 0 };
    static readonly int[] WhiteThreeLine = { 0, 2, 2, 2, 0 };
    static readonly int[] BlackThreeSenterLineFlont = { 0, 1, 1, 0, 1, 0 };
    static readonly int[] BlackThreeSenterLineBack = { 0, 1, 0, 1, 1, 0 };
    static readonly int[] WhiteThreeSenterLineFlont = { 0, 2, 2, 0, 2, 0 };
    static readonly int[] WhiteThreeSenterLineBack = { 0, 2, 0, 2, 2, 0 };
    private static List<int> BlackThreeLineArray = new List<int>();
    private static int BlackFourLineFlag = 0;
    private static int WhiteFourLineFlag = 0;
    private static int WhiteThreeLineFlag = 0;
    private static bool[] FourContinue = { false, false, false, false, false, false, false, false };
    private static int FourContinuePlace = 0;
    private static bool BlackThreeLineErrorFlag = false;
    public static int ScanResult()
    {
        int[] ThreeScale = new int[6];

        //グローバル変数の初期化を行う
        int[] viewsArray = new int[5];
        BlackFourLineFlag = 0;
        WhiteFourLineFlag = 0;
        WhiteThreeLineFlag = 0;
        BlackThreeLineErrorFlag = false;
        BlackThreeLineArray.Clear();
        FourContinue = new System.Boolean[8] { false, false, false, false, false, false, false, false };
        FourContinuePlace = 0;

        for (int i = 0; i < 19; i++)
        {
            for (int j = 0; j < 19 - 4; j++)
            {
                FourContinuePlace = 0;
                //縦のライン確認
                viewsArray = new int[5] { bord[j, i], bord[j + 1, i], bord[j + 2, i], bord[j + 3, i], bord[j + 4, i] };
                ThreeScale = new int[6] { j + 1, i, j + 2, i, j + 3, i };
                ArrayFlag(viewsArray, ThreeScale);
                if (j > 0 && j < 14)
                {
                    viewsArray = new int[6] { bord[j, i], bord[j + 1, i], bord[j + 2, i], bord[j + 3, i], bord[j + 4, i], bord[j + 5, i] };
                    ThreeScale = new int[8] { j + 1, i, j + 2, i, j + 3, i, j + 4, i };
                    ThreeSenterLine(viewsArray, ThreeScale);

                }
                FourContinuePlace = 1;
                //横のライン確認
                viewsArray = new int[5] { bord[i, j + 0], bord[i, j + 1], bord[i, j + 2], bord[i, j + 3], bord[i, j + 4] };
                ThreeScale = new int[6] { i, j + 1, i, j + 2, i, j + 3 };
                ArrayFlag(viewsArray, ThreeScale);
                if (j > 0 && j < 14)
                {
                    viewsArray = new int[6] { bord[i, j + 0], bord[i, j + 1], bord[i, j + 2], bord[i, j + 3], bord[i, j + 4], bord[i, j + 5] };
                    ThreeScale = new int[8] { i, j + 1, i, j + 2, i, j + 3, i, j + 4 };
                    ThreeSenterLine(viewsArray, ThreeScale);

                }

                if (i > 3 && i < 18 && i - 3 > j)
                {
                    FourContinuePlace = 2;
                    //「／」のライン確認
                    viewsArray = new int[5] { bord[j, i - j], bord[j + 1, i - j - 1], bord[j + 2, i - j - 2], bord[j + 3, i - j - 3], bord[j + 4, i - j - 4] };
                    ThreeScale = new int[6] { j + 1, i - j - 1, j + 2, i - j - 2, j + 3, i - j - 3 };
                    ArrayFlag(viewsArray, ThreeScale);
                    if (i > 4 && i < 17 && i - 4 > j)
                    {
                        viewsArray = new int[6] { bord[j, i - j], bord[j + 1, i - j - 1], bord[j + 2, i - j - 2], bord[j + 3, i - j - 3], bord[j + 4, i - j - 4], bord[j + 5, i - j - 5] };
                        ThreeScale = new int[8] { j + 1, i - j - 1, j + 2, i - j - 2, j + 3, i - j - 3, i + 4, i - j - 4 };
                        ThreeSenterLine(viewsArray, ThreeScale);
                    }
                    FourContinuePlace = 3;
                    viewsArray = new int[5] { bord[18 - i + j, 18 - j], bord[18 - i + j + 1, 18 - j - 1], bord[18 - i + j + 2, 18 - j - 2], bord[18 - i + j + 3, 18 - j - 3], bord[18 - i + j + 4, 18 - j - 4] };
                    ThreeScale = new int[6] { 18 - i + j + 1, 18 - j - 1, 18 - i + j + 2, 18 - j - 2, 18 - i + j + 3, 18 - j - 3 };
                    ArrayFlag(viewsArray, ThreeScale);
                    if (i > 4 && i < 17 && i - 4 > j)
                    {
                        viewsArray = new int[6] { bord[18 - i + j, 18 - j], bord[18 - i + j + 1, 18 - j - 1], bord[18 - i + j + 2, 18 - j - 2], bord[18 - i + j + 3, 18 - j - 3], bord[18 - i + j + 4, 18 - j - 4], bord[18 - i + j + 5, 18 - j - 5] };
                        ThreeScale = new int[8] { 18 - i + j + 1, 18 - j - 1, 18 - i + j + 2, 18 - j - 2, 18 - i + j + 3, 18 - j - 3, 18 - i + j + 4, 18 - j - 4 };
                        ThreeSenterLine(viewsArray, ThreeScale);
                    }

                    FourContinuePlace = 4;
                    //「＼」のライン確認
                    viewsArray = new int[5] { bord[18 - i + j, j], bord[18 - i + j + 1, j + 1], bord[18 - i + j + 2, j + 2], bord[18 - i + j + 3, j + 3], bord[18 - i + j + 4, j + 4] };
                    ThreeScale = new int[6] { 18 - i + j + 1, j + 1, 18 - i + j + 2, j + 2, 18 - i + j + 3, j + 3 };
                    ArrayFlag(viewsArray, ThreeScale);
                    if (i > 4 && i < 17 && i - 4 > j)
                    {
                        viewsArray = new int[6] { bord[18 - i + j, j], bord[18 - i + j + 1, j + 1], bord[18 - i + j + 2, j + 2], bord[18 - i + j + 3, j + 3], bord[18 - i + j + 4, j + 4], bord[18 - i + j + 5, j + 5] };
                        ThreeScale = new int[8] { 18 - i + j + 1, j + 1, 18 - i + j + 2, j + 2, 18 - i + j + 3, j + 3, 18 - i + j + 4, j + 4 };
                        ThreeSenterLine(viewsArray, ThreeScale);
                    }
                    FourContinuePlace = 5;
                    viewsArray = new int[5] { bord[j, 18 - i + j], bord[j + 1, 18 - i + j + 1], bord[j + 2, 18 - i + j + 2], bord[j + 3, 18 - i + j + 3], bord[j + 4, 18 - i + j + 4] };
                    ThreeScale = new int[6] { j + 1, 18 - i + j + 1, j + 2, 18 - i + j + 2, j + 3, 18 - i + j + 3 };
                    ArrayFlag(viewsArray, ThreeScale);
                    if (i > 4 && i < 17 && i - 4 > j)
                    {
                        viewsArray = new int[6] { bord[j, 18 - i + j], bord[j + 1, 18 - i + j + 1], bord[j + 2, 18 - i + j + 2], bord[j + 3, 18 - i + j + 3], bord[j + 4, 18 - i + j + 4], bord[j + 5, 18 - i + j + 5] };
                        ThreeScale = new int[8] { j + 1, 18 - i + j + 1, j + 2, 18 - i + j + 2, j + 3, 18 - i + j + 3, j + 4, 18 - i - j + 4 };
                        ThreeSenterLine(viewsArray, ThreeScale);
                    }
                }
            }
            if (i < 19 - 4)
            {
                FourContinuePlace = 6;
                //ななめの一番最後だけ実行する
                viewsArray = new int[5] { bord[i, i], bord[i + 1, i + 1], bord[i + 2, i + 2], bord[i + 3, i + 3], bord[i + 4, i + 4] };
                ThreeScale = new int[6] { i + 1, i + 1, i + 2, i + 2, i + 3, i + 3 };
                ArrayFlag(viewsArray, ThreeScale);
                if (i > 0 && i < 14)
                {
                    viewsArray = new int[6] { bord[i, i], bord[i + 1, i + 1], bord[i + 2, i + 2], bord[i + 3, i + 3], bord[i + 4, i + 4], bord[i + 5, i + 5] };
                    ThreeScale = new int[8] { i + 1, i + 1, i + 2, i + 2, i + 3, i + 3, i + 4, i + 4 };
                    ThreeSenterLine(viewsArray, ThreeScale);
                }
                FourContinuePlace = 7;
                viewsArray = new int[5] { bord[i, 18 - i], bord[i + 1, 18 - i - 1], bord[i + 2, 18 - i - 2], bord[i + 3, 18 - i - 3], bord[i + 4, 18 - i - 4] };
                ThreeScale = new int[6] { i + 1, 18 - i - 1, i + 2, 18 - i - 2, i + 3, 18 - i - 3 };
                ArrayFlag(viewsArray, ThreeScale);
                if (i > 0 && i < 14)
                {
                    viewsArray = new int[6] { bord[i, 18 - i], bord[i + 1, 18 - i - 1], bord[i + 2, 18 - i - 2], bord[i + 3, 18 - i - 3], bord[i + 4, 18 - i - 4], bord[i + 5, 18 - i - 5] };
                    ThreeScale = new int[8] { i + 1, 18 - i - 1, i + 2, 18 - i - 2, i + 3, 18 - i - 3, i + 4, 18 - i - 4 };
                    ThreeSenterLine(viewsArray, ThreeScale);
                }
            }

        }
        blackScore = "-";
        //黒が３＊３を置く場合は無効にする
        if (BlackThreeLineErrorFlag == true)
        {
            return 1;
        }
        //各if文で３，４個並んでいるかのスコアを表示させる。
        if (BlackThreeLineArray.Count != 0)
        {
            blackScore = "3";
        }
        if (BlackFourLineFlag == 1)
        {
            blackScore = "4";
        }
        if (BlackFourLineFlag == 1 && BlackThreeLineArray.Count != 0)
        {
            blackScore = "43";
        }
        if (BlackFourLineFlag > 1)
        {
            blackScore = "44";
        }
        whiteScore = "-";
        if (WhiteThreeLineFlag == 1)
        {
            whiteScore = "3";
        }
        if (WhiteThreeLineFlag > 1)
        {
            whiteScore = "33";
        }
        if (WhiteFourLineFlag == 1)
        {
            whiteScore = "4";
        }
        if (WhiteFourLineFlag == 1 && WhiteThreeLineFlag != 0)
        {
            whiteScore = "43";
        }
        if (WhiteFourLineFlag > 1)
        {
            whiteScore = "44";
        }

        return 0;

    }

    //フラグの判定を行うだけのメソッド。黒の３＊３対策として１０進数で配列に保存している。
    private static void ArrayFlag(int[] viewsArray, int[] threeScale)
    {
        int BlackFourCount = 0;
        int WhiteFourCount = 0;

        for (int i = 0; i < 5; i++)
        {
            if (viewsArray[i] == 1) BlackFourCount++;
            if (viewsArray[i] == 2) WhiteFourCount++;
        }

        if (FourContinue[FourContinuePlace])
        {

            if (BlackFourCount == 4 && WhiteFourCount == 0)
            {

                BlackFourLineFlag--;
            }
            if (WhiteFourCount == 4 && BlackFourCount == 0)
            {
                WhiteFourLineFlag--;
            }
            FourContinue[FourContinuePlace] = false;
        }
        if (BlackFourCount == 5)
        {
            winnerText = "黒勝利";
            endFlag = true;
        }
        if (WhiteFourCount == 5)
        {
            winnerText = "白勝利";
            endFlag = true;
        }
        if (BlackFourCount == 4 && WhiteFourCount == 0)
        {
            BlackFourLineFlag++;
            FourContinue[FourContinuePlace] = true;
        }
        if (WhiteFourCount == 4 && BlackFourCount == 0)
        {
            WhiteFourLineFlag++;
            FourContinue[FourContinuePlace] = true;
        }
        if (viewsArray[0] == BlackThreeLine[0] && viewsArray[1] == BlackThreeLine[1] && viewsArray[2] == BlackThreeLine[2] && viewsArray[3] == BlackThreeLine[3] && viewsArray[4] == BlackThreeLine[4])
        {
            for (int x = 0; x < BlackThreeLineArray.Count; x++)
            {
                if (BlackThreeLineArray[x] == (threeScale[0]) * 100 + (threeScale[1]) || BlackThreeLineArray[x] == (threeScale[2]) * 100 + (threeScale[3]) || BlackThreeLineArray[x] == (threeScale[4]) * 100 + (threeScale[5]))
                {
                    BlackThreeLineErrorFlag = true;
                }
            }
            BlackThreeLineArray.Add(threeScale[0] * 100 + threeScale[1]);
            BlackThreeLineArray.Add(threeScale[2] * 100 + threeScale[3]);
            BlackThreeLineArray.Add(threeScale[4] * 100 + threeScale[5]);

        }
        if (viewsArray[0] == WhiteThreeLine[0] && viewsArray[1] == WhiteThreeLine[1] && viewsArray[2] == WhiteThreeLine[2] && viewsArray[3] == WhiteThreeLine[3] && viewsArray[4] == WhiteThreeLine[4])
        {
            WhiteThreeLineFlag++;
        }
    }
    //中抜き３つの例外処理
    private static void ThreeSenterLine(int[] viewsArray, int[] threeScale)
    {
        if (viewsArray[0] == WhiteThreeSenterLineFlont[0] && viewsArray[1] == WhiteThreeSenterLineFlont[1] && viewsArray[2] == WhiteThreeSenterLineFlont[2] && viewsArray[3] == WhiteThreeSenterLineFlont[3] && viewsArray[4] == WhiteThreeSenterLineFlont[4] && viewsArray[5] == WhiteThreeSenterLineFlont[5])
        {
            WhiteThreeLineFlag++;
        }
        if (viewsArray[0] == WhiteThreeSenterLineBack[0] && viewsArray[1] == WhiteThreeSenterLineBack[1] && viewsArray[2] == WhiteThreeSenterLineBack[2] && viewsArray[3] == WhiteThreeSenterLineBack[3] && viewsArray[4] == WhiteThreeSenterLineBack[4] && viewsArray[5] == WhiteThreeSenterLineBack[5])
        {
            WhiteThreeLineFlag++;
        }
        if (viewsArray[0] == BlackThreeSenterLineFlont[0] && viewsArray[1] == BlackThreeSenterLineFlont[1] && viewsArray[2] == BlackThreeSenterLineFlont[2] && viewsArray[3] == BlackThreeSenterLineFlont[3] && viewsArray[4] == BlackThreeSenterLineFlont[4] && viewsArray[5] == BlackThreeSenterLineFlont[5])
        {
            for (int x = 0; x < BlackThreeLineArray.Count; x++)
            {
                if (BlackThreeLineArray[x] == (threeScale[0]) * 100 + (threeScale[1]) || BlackThreeLineArray[x] == (threeScale[2]) * 100 + (threeScale[3]) || BlackThreeLineArray[x] == (threeScale[6]) * 100 + (threeScale[7]))
                {
                    BlackThreeLineErrorFlag = true;
                }
            }
            BlackThreeLineArray.Add(threeScale[0] * 100 + threeScale[1]);
            BlackThreeLineArray.Add(threeScale[2] * 100 + threeScale[3]);
            BlackThreeLineArray.Add(threeScale[6] * 100 + threeScale[7]);
        }
        if (viewsArray[0] == BlackThreeSenterLineBack[0] && viewsArray[1] == BlackThreeSenterLineBack[1] && viewsArray[2] == BlackThreeSenterLineBack[2] && viewsArray[3] == BlackThreeSenterLineBack[3] && viewsArray[4] == BlackThreeSenterLineBack[4] && viewsArray[5] == BlackThreeSenterLineBack[5])
        {
            for (int x = 0; x < BlackThreeLineArray.Count; x++)
            {
                if (BlackThreeLineArray[x] == (threeScale[0]) * 100 + (threeScale[1]) || BlackThreeLineArray[x] == (threeScale[4]) * 100 + (threeScale[5]) || BlackThreeLineArray[x] == (threeScale[6]) * 100 + (threeScale[7]))
                {
                    BlackThreeLineErrorFlag = true;
                }
            }
            BlackThreeLineArray.Add(threeScale[0] * 100 + threeScale[1]);
            BlackThreeLineArray.Add(threeScale[4] * 100 + threeScale[5]);
            BlackThreeLineArray.Add(threeScale[6] * 100 + threeScale[7]);
        }
    }
    #endregion
}
