using UnityEngine;
using UnityEditor;
using static UnityEditor.EditorGUILayout;
using static UnityEngine.GUILayout;

public class OrganizeObject : EditorWindow
{
    [MenuItem("Plugins/OrganizeObject")]
    public static void ShowWindow() {
        GetWindow(typeof(OrganizeObject));
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

    bool relation = false;

    void OnGUI() {
        Space();
        prefab = ObjectField("Prefab", prefab, typeof(GameObject), false) as GameObject;

        Space();
        selectedDimension = Popup("Dimension", selectedDimension, Dimension);
        Space();

        StartPos = Vector3Field("StartPos", StartPos);
        OffsetVector = Vector3Field("Offset", OffsetVector);
        if(selectedDimension == 1) Label("   ※Y-coordinate is always ０.", EditorStyles.miniLabel);

        switch(selectedDimension) {
            case 0:
                Space();
                Quantity = IntField("Quantity", Quantity);
                Space();

                //オプションで、名前の番号の付け方を変更
                OptionalSettings = BeginToggleGroup("Optional Settings", OptionalSettings);
                selectedOder = Popup("Oder mode", selectedOder, Oder);
                num0 = IntField("Start number (Oder)", num0);
                EndToggleGroup();

                Space();
                relation = Toggle("Keep relation", relation);

                if(Button("Instantiate")) {
                    for(int i = 0; i < Quantity; i++) {
                        GameObject x;
                        //生成
                        if(relation) {
                            x = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                        } else {
                            x = Instantiate(prefab) as GameObject;
                        }
                        //位置変更
                        x.transform.position = StartPos + (OffsetVector * i);
                        //名前付け
                        if(OptionalSettings) {
                            if(selectedOder == 0) {
                                x.name = (num0 + i).ToString();
                            } else if(selectedOder == 1) {
                                x.name = (num0 - i).ToString();
                            }
                        }
                    }
                }
                break;

            case 1:
                Space();
                Label("Cube Arrangement", EditorStyles.boldLabel);
                Ax = IntField("Arrangement_x", Ax);
                Az = IntField("Arrangement_z", Az);
                LabelField("The number of Cube : ", (Ax * Az).ToString());
                Space();

                //オプションで、名前の番号の付け方を変更
                OptionalSettings = BeginToggleGroup("Optional Settings", OptionalSettings);
                selectedOder = Popup("Oder mode", selectedOder, Oder);
                num0 = IntField("Start number (Oder_x)", num0);
                num1 = IntField("Start number (Oder_z)", num1);
                reverse_XY = Toggle("Reverse (X ⇄ Y)", reverse_XY);
                EndToggleGroup();


                Space();
                relation = Toggle("Keep relation", relation);

                if(Button("Instantiate")) {
                    for(int i = 0; i < Ax; i++) {
                        for(int k = 0; k < Az; k++) {
                            GameObject x;
                            //生成
                            if(relation) {
                                x = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                            } else {
                                x = Instantiate(prefab) as GameObject;
                            }
                            x.transform.position = StartPos + new Vector3(OffsetVector.x * k, 0, OffsetVector.z * i);

                            //名前付け
                            if(OptionalSettings) {
                                if(reverse_XY) {
                                    if(selectedOder == 0) {
                                        x.name = (num1 + k).ToString() + "-" + (num0 + i).ToString();
                                    } else if(selectedOder == 1) {
                                        x.name = (num1 - k).ToString() + "-" + (num0 - i).ToString();
                                    }
                                } else {
                                    if(selectedOder == 0) {
                                        x.name = (num1 + i).ToString() + "-" + (num0 + k).ToString();
                                    } else if(selectedOder == 1) {
                                        x.name = (num1 - i).ToString() + "-" + (num0 - k).ToString();
                                    }
                                }
                            }
                        }
                    }
                }
                break;
        }

        Space();
        if(Button("Close Window")) Close();

    }
}