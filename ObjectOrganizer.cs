using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;
using static UnityEngine.GUILayout;

public class ObjectOrganazer : EditorWindow
{
    Vector2 scrollPosition = new Vector2(0, 0);

    [MenuItem("Plugins/ObjectOrganazer")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ObjectOrganazer));
    }

    GameObject prefab;

    string[] Dimension = { "1D", "2D" };
    int selectedDimension = 0;

    string[] Oder = { "Ascending Order", "Descending Order" };
    int selectedOder;

    Vector3 StartPos, OffsetVector;
    int Quantity = 1;
    bool OptionalSettings = false;
    int num0 = 0, num1 = 0;
    bool reverse_XY = false;

    int Ax, Az;

    bool asChild = false;
    GameObject Parent;

    bool relation = false;




    void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        Space();

        //生成するプレハブをセットする
        prefab = ObjectField("Prefab", prefab, typeof(GameObject), false) as GameObject;

        //子として生成するかどうか
        asChild = BeginToggleGroup("Creat as Child", asChild);
        Parent = ObjectField("ParentObject", Parent, typeof(GameObject), true) as GameObject;
        EndToggleGroup();

        Space();

        //次元数を選択する
        selectedDimension = Popup("Dimension", selectedDimension, Dimension);

        Space();

        //開始地点、差、をセットする
        StartPos = Vector3Field("StartPos", StartPos);
        OffsetVector = Vector3Field("Offset", OffsetVector);
        if (selectedDimension == 1) Label("   ※Y-coordinate is always ０.", EditorStyles.miniLabel);

        //以下は、次元ごとに処理が違うので分岐する
        switch (selectedDimension)
        {
            //1次元の時
            case 0:
                Space();

                //生成するプレハブの数
                Quantity = IntField("Quantity", Quantity);

                Space();

                //オプションで、名前の番号の付け方を変更
                OptionalSettings = BeginToggleGroup("Optional Settings", OptionalSettings);
                selectedOder = Popup("Oder mode", selectedOder, Oder);
                num0 = IntField("Start number (Oder)", num0);
                EndToggleGroup();

                Space();

                //Cloneとして生成するか、Pfrefabとして生成するか
                relation = Toggle("Keep relation", relation);

                //生成ボタンが押された時の処理
                if (Button("Instantiate"))
                {
                    //エラーチェック
                    if (prefab == null)
                    {
                        Debug.LogError("Not set a prefab. Instantiating prefab, it is required to set a prefab");
                        return;
                    }

                    for (int i = 0; i < Quantity; i++)
                    {
                        GameObject x;

                        //生成
                        if (relation)
                        {
                            x = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                        }
                        else
                        {
                            x = Instantiate(prefab) as GameObject;
                        }

                        //位置変更
                        x.transform.position = StartPos + (OffsetVector * i);

                        //名前付け
                        if (OptionalSettings)
                        {
                            if (selectedOder == 0)
                            {
                                x.name = (num0 + i).ToString();
                            }
                            else if (selectedOder == 1)
                            {
                                x.name = (num0 - i).ToString();
                            }
                        }

                        //親を取得
                        if (asChild) x.transform.parent = Parent.transform;
                    }
                }
                break;

            case 1:
                Space();

                //X、Y軸ごとの並べ方を入力する
                Label("Arrangement", EditorStyles.boldLabel);
                Ax = IntField("Arrangement_x", Ax);
                Az = IntField("Arrangement_z", Az);
                LabelField("The number of Cube : ", (Ax * Az).ToString());

                //オプションで、名前の番号の付け方を変更
                OptionalSettings = BeginToggleGroup("Optional Settings", OptionalSettings);
                selectedOder = Popup("Oder mode", selectedOder, Oder);
                num0 = IntField("Start number (Oder_x)", num0);
                num1 = IntField("Start number (Oder_z)", num1);
                reverse_XY = Toggle("Reverse (X ⇄ Y)", reverse_XY);
                EndToggleGroup();

                //関係性を維持するかどうか（Cloneか否か）
                relation = Toggle("Keep relation", relation);

                //生成開始
                if (Button("Instantiate"))
                {
                    //エラーチェック
                    if (prefab == null)
                    {
                        Debug.Log("Not set a prefab. Instantiating prefab, it is required to set a prefab");
                        return;
                    }
                    if (asChild && (Parent == null))
                    {
                        Debug.Log("Not set a parent. Creating as child, it is required to set a parent GameObject");
                        return;
                    }

                    for (int i = 0; i < Ax; i++)
                    {
                        for (int k = 0; k < Az; k++)
                        {
                            GameObject x; //インスタンス

                            //生成
                            if (relation)
                            {
                                x = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                            }
                            else
                            {
                                x = Instantiate(prefab) as GameObject;
                            }

                            //座標指定
                            x.transform.position = StartPos + new Vector3(OffsetVector.x * k, 0, OffsetVector.z * i);

                            //名前付け
                            if (OptionalSettings)
                            {
                                if (reverse_XY)
                                {
                                    if (selectedOder == 0)
                                    {
                                        x.name = (num1 + k).ToString() + "-" + (num0 + i).ToString();
                                    }
                                    else if (selectedOder == 1)
                                    {
                                        x.name = (num1 - k).ToString() + "-" + (num0 - i).ToString();
                                    }
                                }
                                else
                                {
                                    if (selectedOder == 0)
                                    {
                                        x.name = (num1 + i).ToString() + "-" + (num0 + k).ToString();
                                    }
                                    else if (selectedOder == 1)
                                    {
                                        x.name = (num1 - i).ToString() + "-" + (num0 - k).ToString();
                                    }
                                }
                            }

                            //親を取得
                            if (asChild) x.transform.parent = Parent.transform;
                        }
                    }
                }
                break;
        }

        Space();
        if (Button("Close Window")) Close();

        EditorGUILayout.EndScrollView();
    }
}
