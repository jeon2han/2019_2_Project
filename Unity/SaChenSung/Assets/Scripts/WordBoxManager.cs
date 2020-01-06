using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 경로 저장 구조체
public struct Pos
{
    public int x;
    public int y;
    public Pos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public void setPos(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
public class WordBoxManager : MonoBehaviour
{
    public static WordBoxManager wbm;
    static GameObject prefab;
    GameObject gameBoard;
    WordBox next;
    WordBox wordBox; // 첫번째 클릭한 wordBox id 저장 -> 나중에 클릭한 wordbox id랑 비교 예정
    public WordBox[,] wordBoxes; // 단어 아이디 게임오브젝트 등 정보가 유지될 객체
    WordDeck[] wordDecks;
    string[] imageName = { "box", "box", "box", "box" };

    // 워드 박스 시작 좌표
    int x_position = -540; // 좌표값 보고 건들일것
    int y_position = 160;
    const int x_const_position = -540;
    const int y_const_position = 160;

    // 박스 크기
    const int box_x_size = 350;
    const int box_y_size = 170;

    // 워드박스 가로 세로 길이
    int row = 4; // 가로
    int col = 4; // 세로
    public int wordNum; // 단어 개수 // 짝수여야 함


    //사천성
    string[,] tmap = new string[6, 6];
    bool[,] check = new bool[6, 6];
    Pos[] path_pos = new Pos[15];
    private readonly int[] posX = { 0, -1, 0, 1 };
    private readonly int[] posY = { 1, 0, -1, 0 };
    private readonly int ROW_SIZE = 6;
    private readonly int COL_SIZE = 6;


    public string[,] Copy(string[,] tmap, WordBox[,] w1)
    {
        for (int i = 0; i < row + 2; i++)
        {
            for (int j = 0; j < col + 2; j++)
            {
                tmap[i, j] = ".";
            }
        }
        for (int i = 1; i < row + 1; i++)
        {
            for (int j = 1; j < col + 1; j++)
            {
                tmap[i, j] = w1[i - 1, j - 1].id;

            }
        }
        return tmap;
    }

    private bool isRange(int cx, int cy)
    {
        if (cx >= 0 && cx < ROW_SIZE && cy >= 0 && cy < COL_SIZE)
            return true;
        else
            return false;
    }

    private void init()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                check[i, j] = false;
            }
        }

        for (int i = 0; i < 15; i++)
        {
            path_pos[i] = new Pos();
            path_pos[i].x = -1;
        }
    }

    private bool check_value(int x1, int x2)
    {
        if (x1 == 0)
        {
            if (x2 == 0)
                return true;
            else
                return false;
        }
        else
        {
            bool is_x1_plus = x1 > 0 ? true : false;
            bool is_x2_plus = x2 > 0 ? true : false;
            if (is_x1_plus == is_x2_plus)
                return true;
            else
                return false;
        }
    }

    private void dfs_find(Pos move, Pos to, int depth, int pre_dir, ref bool pre_bool, int path_depth)
    {
        if (pre_bool)
            return;

        if (depth > 2)
            return;

        else if (depth == 2)
        {
            int diff_x = to.x - move.x;
            int diff_y = to.y - move.y;

            if ((diff_x == 0 || diff_y == 0) && check_value(diff_x, posX[pre_dir]) && check_value(diff_y, posY[pre_dir]))
            {
                int incre_depth = path_depth;

                // x가 고정일 때
                if (diff_x == 0)
                {
                    if (move.y < to.y)
                    {
                        for (int i = move.y; i <= to.y; i++)
                        {
                            if (!((i != to.x && tmap[move.x, i] == ".") || (i == to.y)))
                                return;

                            path_pos[incre_depth + 1].setPos(move.x, i);
                            incre_depth++;
                        }
                    }
                    else
                    {
                        for (int i = move.y; i >= to.y; i--)
                        {
                            if (!((i != to.x && tmap[move.x, i] == ".") || (i == to.y)))
                                return;
                            path_pos[incre_depth + 1].setPos(move.x, i);
                            incre_depth++;
                        }
                    }
                }

                // y가 고정일 때
                else if (diff_y == 0)
                {
                    if (move.x < to.x)
                    {
                        for (int i = move.x; i <= to.x; i++)
                        {
                            if (!((i != to.x && tmap[i, move.y] == ".") || (i == to.x)))
                                return;
                            path_pos[incre_depth + 1].setPos(i, move.y);
                            incre_depth++;
                        }
                    }
                    else
                    {
                        for (int i = move.x; i >= to.x; i--)
                        {
                            if (!((i != to.x && tmap[i, move.y] == ".") || (i == to.x)))
                                return;
                            path_pos[incre_depth + 1].setPos(i, move.y);
                            incre_depth++;
                        }
                    }
                }

                pre_bool = true;
                return;
            }
        }
        // depth == 0 or depth == 1
        else
        {
            for (int i = 0; i < 4; i++)
            {
                int cx = move.x + posX[i];
                int cy = move.y + posY[i];
                // out of bound
                if (!isRange(cx, cy))
                    continue;

                // depth == 0 or 1 일때의 정답.
                if (to.x == cx && to.y == cy)
                {
                    pre_bool = true;
                    path_pos[path_depth + 1].setPos(cx, cy);
                    return;
                }

                if (tmap[cx, cy] == "." && !check[cx, cy])
                {
                    // 가는 방향이 같을 때 => move
                    if (pre_dir == i)
                    {
                        check[cx, cy] = true;
                        path_pos[path_depth + 1].setPos(cx, cy);
                        dfs_find(new Pos(cx, cy), to, depth, i, ref pre_bool, path_depth + 1);
                        if (pre_bool)
                            return;
                        check[cx, cy] = false;
                        path_pos[path_depth + 1].setPos(-1, 0);
                    }

                    // 가는 방향이 다를 때
                    else
                    {
                        // 처음 선택한 곳에서 움직일 때 => move
                        if (pre_dir < 0)
                        {
                            check[cx, cy] = true;
                            path_pos[path_depth + 1].setPos(cx, cy);
                            dfs_find(new Pos(cx, cy), to, depth, i, ref pre_bool, path_depth + 1);
                            if (pre_bool)
                                return;
                            check[cx, cy] = false;
                            path_pos[path_depth + 1].setPos(-1, 0);
                        }

                        // 처음 선택한 곳에서 움직이지 않을 때 => move
                        else
                        {
                            check[cx, cy] = true;
                            path_pos[path_depth + 1].setPos(cx, cy);
                            dfs_find(new Pos(cx, cy), to, depth + 1, i, ref pre_bool, path_depth + 1);
                            if (pre_bool)
                                return;
                            check[cx, cy] = false;
                            path_pos[path_depth + 1].setPos(-1, 0);
                        }
                    }
                }
            }
        }
    }
    public bool solve(WordBox from, WordBox to, int seg, string mychar)
    {
        Debug.Log("solve");
        bool check = false;
        //int depth = 0;
        Pos sp = new Pos(from.row, from.col);
        Pos ep = new Pos(to.row, to.col);

        init();

        path_pos[0].setPos(from.row, from.col);

        dfs_find(sp, ep, 0, -1, ref check, 0);

        if (check)
        {
            for (int i = 0; i < 10; i++)
            {
                if (path_pos[i].x == -1)
                    break;
                Debug.Log(path_pos[i].x.ToString() + "   " + path_pos[i].y.ToString());
            }
            return true;
        }
        else
            return false;

        return false;
    }
    public WordBoxManager()
    {
        if (wbm == null)
            wbm = this;
        prefab = Resources.Load<GameObject>("WordBox");
        wordBox = new WordBox(); // 정답 체크용 객체
        next = new WordBox();
    }

    public void CreateBtns(int level, GameObject gameObject) // 워드박스 생성
    {
        gameBoard = gameObject;
        wordNum = row * col;
        wordDecks = SaveDeck.getDeck(); // 덱 가져오기
        wordBoxes = new WordBox[row, col]; // 단어 담을 객체 배열

        RemainWordBox();

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                GameObject button = Instantiate(prefab);
                //단어박스 저장
                int position = (row * i) + j; // 현재 배열 위치
                wordBoxes[i, j] = new WordBox(button, wordDecks[position].word, wordDecks[position].id);
                GameObject instance = wordBoxes[i, j].instance;
                wordBoxes[i, j].row = i + 1;
                wordBoxes[i, j].col = j + 1;

                //버튼에 이미지 넣기
                int r = Random.Range(0, 4);
                Sprite btnSprite = Resources.Load<Sprite>(imageName[r]);
                Image image = instance.GetComponent<Image>();
                image.sprite = btnSprite;

                //위치 잡기
                RectTransform btnpos = instance.GetComponent<RectTransform>();
                instance.transform.position = gameObject.transform.position;
                btnpos.sizeDelta = new Vector2(box_x_size, box_y_size);
                btnpos.SetParent(gameObject.transform);
                btnpos.anchoredPosition = new Vector2(x_position, y_position);
                btnpos.localScale = new Vector2(1, 1);
                x_position += box_x_size;

                //버튼 글자 넣기
                Text text = instance.GetComponentInChildren<Text>();
                text.text = wordBoxes[i, j].word;
                text.fontSize = 100;
                text.font = Resources.Load<Font>("Typo");

                // Text 오브젝트 공간 조정
                ContentSizeFitter sizeFitter = instance.transform.Find("Text").gameObject.AddComponent<ContentSizeFitter>();
                sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

                // 버튼 클릭 리스너 등록
                Button btn = instance.GetComponent<Button>();
                btn.onClick.AddListener(() => { EffectSoundManager.instance.PlayPlayerClickedSound(); }); // 클릭 소리
                if (SaveLevel.level == 1)
                {
                    btn.onClick.AddListener(() => { CorrectCheck_Level_one(instance); }); // 비슷한 단어 찾기 정답 체크 함수
                }
                else if (SaveLevel.level == 2)
                {
                    btn.onClick.AddListener(() => { CorrectCheck_Level_two(instance); }); // 사천성 정답 체크 함수
                }

            }
            x_position = -540;
            y_position -= (box_y_size + 15);
        }
        // GameObject.Find("WordBox").SetActive(false);
        //초기화 다시해줘야 경로 출력 때 사용
        x_position = x_const_position;
        y_position = y_const_position;

        Copy(tmap, wordBoxes);
    }

    void CorrectCheck_Level_one(GameObject clickObject) //정답체크 
    {
        Debug.Log("CorrectCheck");
        WordBox clickWordBox = FindWordBox(clickObject);

        if (wordBox.id == "") //클릭된게 없으면
        {
            wordBox.id = clickWordBox.id;
            wordBox.instance = clickWordBox.instance;
            OutLineOnOFF(wordBox.instance);
        }
        else
        {
            if (wordBox.id == clickWordBox.id && wordBox.instance != clickObject) // 현재 클릭된 거랑 이전에 클릭된게 같으면 정답처리
            {
                Debug.Log("맞음");
                EffectSoundManager.instance.PlayCorrectSound();
                wordNum -= 2;
                wordBox.instance.SetActive(false);
                FindWordBox(wordBox.instance).active = false;
                clickWordBox.instance.SetActive(false);
                clickWordBox.active = false;
                RemainWordBox();
            }
            else
            {
                Debug.Log("틀림");
                EffectSoundManager.instance.PlayWrongSound();
                OutLineOnOFF(wordBox.instance);
            }
            wordBox = new WordBox();
        }
    }

    void CorrectCheck_Level_two(GameObject clickObject) //정답체크
    {
        WordBox clickWordBox = FindWordBox(clickObject);

        if (wordBox.id == "") //클릭된게 없으면
        {
            wordBox.id = clickWordBox.id;
            wordBox.row = clickWordBox.row;
            wordBox.col = clickWordBox.col;
            wordBox.instance = clickWordBox.instance;
            OutLineOnOFF(wordBox.instance);
        }
        else
        {
            Debug.Log("start" + wordBox.row + "" + wordBox.col + "" + wordBox.id);
            Debug.Log("clicked" + clickWordBox.row + "" + clickWordBox.col + "" + clickWordBox.id);
            if (wordBox.id == clickWordBox.id && wordBox.instance != clickObject) // 현재 클릭된 거랑 이전에 클릭된게 같으면 정답처리
            {
                bool flag = solve(wordBox, clickWordBox, 0, wordBox.id);
                string mychar = clickWordBox.id;
                if (flag == true)
                {
                    EffectSoundManager.instance.PlayCorrectSound();
                    wordBox.instance.SetActive(false);
                    FindWordBox(wordBox.instance).active = false;
                    clickWordBox.instance.SetActive(false);
                    clickWordBox.active = false;
                    tmap[wordBox.row, wordBox.col] = ".";
                    tmap[clickWordBox.row, clickWordBox.col] = ".";

                    if (wordNum > 2)
                    {
                        PrintRoad();
                        if (Status() == false) SaveState.state = false;
                    }
                    wordNum -= 2;
                    RemainWordBox();
                    Debug.Log("맞음");
                }
                else
                {
                    EffectSoundManager.instance.PlayWrongSound();
                    OutLineOnOFF(wordBox.instance);
                    Debug.Log("틀림");
                }
            }
            else
            {
                EffectSoundManager.instance.PlayWrongSound();
                Debug.Log("틀림");
                OutLineOnOFF(wordBox.instance);
                tmap[wordBox.row + 1, wordBox.col + 1] = wordBox.id;
            }
            wordBox = new WordBox();
        }
        System.GC.Collect();
    }

    WordBox FindWordBox(GameObject gameObject) // 클릭한 오브젝트가 있는 wordbox 찾기
    {
        Debug.Log("FindWordBox");
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (wordBoxes[i, j].instance == gameObject)
                {
                    return wordBoxes[i, j];
                }
            }
        }
        Debug.Log("못찾음");
        return new WordBox();
    }

    void OutLineOnOFF(GameObject gameObject) //외곽선 표시
    {
        if (gameObject.GetComponent<Outline>() == null) // ON
        {
            Outline outline = gameObject.AddComponent<Outline>();
            outline.effectColor = Color.red;
            outline.effectDistance = new Vector2(7, 2);
        }
        else // OFF
        {
            Destroy(gameObject.GetComponent<Outline>());
        }
    }

    void RemainWordBox() // 남은 단어수 표시
    {
        Text text = GameObject.Find("RemindWord").GetComponentInChildren<Text>();
        text.text = "남은패 : " + (wordNum.ToString());
    }

    public void HideOnOff(bool on) // WordBox 감추기
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                if (wordBoxes[i, j].active == true)
                {
                    if (on == true) //보이게
                    {
                        wordBoxes[i, j].instance.SetActive(true);
                    }
                    else // 안보이게
                    {
                        wordBoxes[i, j].instance.SetActive(false);
                    }
                }
            }
        }
    }

    // 경로 출력
    void PrintRoad()
    {
        int path_length = 0; // 경로 길이

        // 경로 길이 찾기
        for (int i = 0; i < path_pos.Length; i++)
        {
            if (path_pos[i].x == -1)
            {
                path_length = i;
                break;
            }
        }

        // 경로가 바로 붙은 직선이면 경로 출력 안함
        if (path_length == 2) return;

        // 경로 출력
        if (path_length == 3) PrintImage(path_pos[0], path_pos[1], path_pos[2]);
        else
        {
            for (int i = 1; i < path_length - 1; i++)
            {
                PrintImage(path_pos[i - 1], path_pos[i], path_pos[i + 1]);
            }
        }
    }

    // 경로 이미지 출력 // 좌표계산 및 출력갯수 문의 필요
    void PrintImage(Pos p0, Pos p1, Pos p2)
    {
        Sprite roadSprite = null;

        int x0 = p0.x, y0 = p0.y; //이전
        int x1 = p1.x, y1 = p1.y; //현재
        int x2 = p2.x, y2 = p2.y; //다음

        if ((x0 == x1) ? ((x0 == x2) ? ((x1 == x2) ? true : false) : false) : false)
        {
            Debug.Log("가로");
            roadSprite = Resources.Load<Sprite>("width");
        }
        else if ((y0 == y1) ? ((y0 == y2) ? ((y1 == y2) ? true : false) : false) : false)
        {
            Debug.Log("세로");
            roadSprite = Resources.Load<Sprite>("height");
        }
        else if (((x0 == x1) ? (x1 < x2 ? true : false) : false) && ((y1 == y2) ? (y0 > y1 ? true : false) : false) || ((x1 == x2) ? (x1 < x0 ? true : false) : false) && ((y0 == y1) ? (y2 > y1 ? true : false) : false))
        {
            Debug.Log("우하");
            roadSprite = Resources.Load<Sprite>("downright");
        }
        else if (((x0 == x1) ? (x1 > x2 ? true : false) : false) && ((y1 == y2) ? (y0 > y1 ? true : false) : false) || ((x1 == x2) ? (x1 > x0 ? true : false) : false) && ((y0 == y1) ? (y2 > y1 ? true : false) : false))
        {
            Debug.Log("상우");
            roadSprite = Resources.Load<Sprite>("upright");
        }
        else if (((x2 == x1) ? (x0 < x1 ? true : false) : false) && ((y0 == y1) ? (y1 > y2 ? true : false) : false) || ((x0 == x1) ? (x1 > x2 ? true : false) : false) && ((y2 == y1) ? (y1 > y0 ? true : false) : false))
        {
            Debug.Log("상좌");
            roadSprite = Resources.Load<Sprite>("upleft");
        }
        else if (((x2 == x1) ? (x0 > x1 ? true : false) : false) && ((y0 == y1) ? (y1 > y2 ? true : false) : false) || ((x0 == x1) ? (x1 < x2 ? true : false) : false) && ((y2 == y1) ? (y1 > y0 ? true : false) : false))
        {
            Debug.Log("좌하");
            roadSprite = Resources.Load<Sprite>("downleft");
        }

        //객체 생성
        GameObject roadObject = Instantiate(prefab);

        //버튼에 이미지 넣기
        Image image = roadObject.GetComponent<Image>();
        image.sprite = roadSprite;

        //버튼 글자 넣기
        Text text = roadObject.GetComponentInChildren<Text>();
        text.text = "";

        //객체 위치 잡기
        //정답 길찾기로 인해 배열 크기가 원래 크기보다 +1씩 되어 있음
        int real_x = x1 - 1;
        int real_y = y1 - 1;

        Debug.Log(real_x);
        Debug.Log(real_y);

        if (real_y < 0)
            x_position -= box_x_size;
        else
        {
            for (int i = 0; i < real_y; i++)
            {
                x_position += box_x_size;
            }
        }

        if (real_x < 0)
            y_position += (box_y_size + 15);
        else
        {
            for (int i = 0; i < real_x; i++)
            {
                y_position -= (box_y_size + 15);
            }
        }

        RectTransform position = roadObject.GetComponent<RectTransform>();
        roadObject.transform.position = gameBoard.transform.position;
        position.sizeDelta = new Vector2(box_x_size, box_y_size + 15);
        position.SetParent(gameBoard.transform);
        position.anchoredPosition = new Vector2(x_position, y_position);
        position.localScale = new Vector2(1, 1);

        Destroy(roadObject, 0.3f); // 0.3초후 삭제

        x_position = x_const_position;
        y_position = y_const_position;
    }

    public void Hint()
    {
        Debug.Log("hint");
        if (SaveLevel.level == 1)
        {
            for (int i = 1; i < ROW_SIZE - 1; i++)
            {
                for (int j = 1; j < COL_SIZE - 1; j++)
                {
                    //선택된 칸이 빈칸이면 패스
                    for (int k = 1; k < ROW_SIZE - 1; k++)
                    {
                        for (int l = 1; l < COL_SIZE - 1; l++)
                        {
                            //시작과 끝이 같은칸이면 패스
                            if (k == i && l == j) continue;
                            //끝의 칸이 빈칸이면 패스
                            if (tmap[k, l] == ".") continue;
                            WordBox to = wordBoxes[i - 1, j - 1];
                            WordBox from = wordBoxes[k - 1, l - 1];

                            if (to.instance.active == false || from.instance.active == false) continue;

                            if (to.id == from.id)
                            {
                                Image imageTo = to.instance.GetComponent<Image>();
                                imageTo.color = new Color32(148, 255, 158, 255);
                                Image imageFrom = from.instance.GetComponent<Image>();
                                imageFrom.color = new Color32(148, 255, 158, 255);
                                return;
                            }
                            else
                                continue;

                        }
                    }
                }
            }
        }
        else
        {
            bool flag = false;
            for (int i = 1; i < ROW_SIZE - 1; i++)
            {
                for (int j = 1; j < COL_SIZE - 1; j++)
                {
                    //선택된 칸이 빈칸이면 패스
                    if (tmap[i, j] == ".") continue;
                    else
                        for (int k = 1; k < ROW_SIZE - 1; k++)
                        {
                            for (int l = 1; l < COL_SIZE - 1; l++)
                            {
                                //시작과 끝이 같은칸이면 패스
                                if (k == i && l == j) continue;
                                //끝의 칸이 빈칸이면 패스
                                if (tmap[k, l] == ".") continue;
                                WordBox to = wordBoxes[i - 1, j - 1];
                                WordBox from = wordBoxes[k - 1, l - 1];

                                if (to.id == from.id)
                                    flag = solve(to, from, 0, tmap[i, j]);

                                if (flag == true)
                                {
                                    Debug.Log(tmap[i, j]);
                                    Image imageTo = to.instance.GetComponent<Image>();
                                    imageTo.color = new Color32(148, 255, 158, 255);
                                    Image imageFrom = from.instance.GetComponent<Image>();
                                    imageFrom.color = new Color32(148, 255, 158, 255);
                                    return;
                                }
                                else
                                    continue;
                            }
                        }
                }
            }
        }

    }

    public bool Status()
    {
        bool flag = false;
        for (int i = 1; i < ROW_SIZE - 1; i++)
        {
            for (int j = 1; j < COL_SIZE - 1; j++)
            {
                //선택된 칸이 빈칸이면 패스
                if (tmap[i, j] == ".") continue;
                else
                    for (int k = 1; k < ROW_SIZE - 1; k++)
                    {
                        for (int l = 1; l < COL_SIZE - 1; l++)
                        {
                            //시작과 끝이 같은칸이면 패스
                            if (k == i && l == j) continue;
                            //끝의 칸이 빈칸이면 패스
                            if (tmap[k, l] == ".") continue;
                            WordBox to = wordBoxes[i - 1, j - 1];
                            WordBox from = wordBoxes[k - 1, l - 1];

                            if (to.id == from.id)
                                flag = solve(to, from, 0, tmap[i, j]);

                            if (flag == true)
                            {
                                return true;
                            }
                            else
                                continue;

                        }
                    }
            }
        }
        return false;
    }
}