import urllib3
import json
import re
import pandas as pd

# 배열 안의 배열 인덱스 위치 찾기
def find_list_index(list, val):
    for i in range(len(list)):
        if list[i].count(val) != 0:
            return i
    return -1

#형태소 분석기
def morphoid_analysis(msg):
    openApiURL = "http://aiopen.etri.re.kr:8000/WiseNLU"
    accessKey = "YOUR-ETRI-KEY"
    analysisCode = "morp"
    text = msg

    word = []
    type = []
    word_list = []

    requestJson = {
        "access_key": accessKey,
        "argument": {
            "text": text,
            "analysis_code": analysisCode
        }
    }

    http = urllib3.PoolManager()

    response = http.request(
        "POST",
        openApiURL,
        headers={"Content-Type": "application/json; charset=UTF-8"},
        body=json.dumps(requestJson)
    )

    content = json.loads(str(response.data, "utf-8"))
    morp = content["return_object"]["sentence"][0]["morp"]

    for i in range(len(morp)):
        word.append(morp[i]["lemma"])
        type.append(morp[i]["type"])

    for i in range(len(word)):
        if (type[i].find("NN") != -1 or type[i] == "VV"):
            word_list.append(word[i])

    return word_list

# Etri-API 어휘간 유사도 분석
def similar_analysis(w1,w2):
    openApiURL = "http://aiopen.etri.re.kr:8000/WiseWWN/WordRel"

    accessKey = "YOUR-ETRI-KEY"

    firstWord = w1
    secondWord = w2

    requestJson = {
        "access_key": accessKey,
        "argument": {
            'first_word': firstWord,
            'second_word': secondWord,
        }
    }

    http = urllib3.PoolManager()
    response = http.request(
        "POST",
        openApiURL,
        headers={"Content-Type": "application/json; charset=UTF-8"},
        body=json.dumps(requestJson)
    )

    content = json.loads(str(response.data, "utf-8"))
    result_word_list = content["return_object"]["WWN WordRelInfo"]["WordRelInfo"]["ShortedPath"]

    if len(result_word_list) == 0 : return -1 # 이름 같은경우 아무것도 없음

    # JSON 결과 단어들에서 LCS : XXXX 이기에 정규식으로 필터링
    korean = re.compile('[^가-힣]+')

    result = ""
    # 결과 중 LCS가 최종 결과
    for i in result_word_list:
        if 'LCS' in i:
            result = korean.sub('', i)

    return result

# 유사 단어 목록 만들기 / input=[차,자동차,...,소방차]
# 어휘 간 유사도 분석을 이용하여 공통 분모 찾기
def find_similar_list(list):
    result_list = []    # 최종 결과 리스트 [[],[],[]] 형태
    LCS_list  = []  # LCS 결과 목록
    temp_list = []
    for i in range(len(list)-1):
        w1 = list[i]
        for j in range(i+1,len(list)):

            w2 = list[j]
            lcs = similar_analysis(w1,w2)

            if lcs == "" or lcs == "물건" or lcs == -1: #LCS가 root면 거리 벡터가 애매하므로 패스, 물건은 너무 많음
                continue

            temp = []
            temp.append(w1)
            temp.append(w2)
            if temp.count(lcs) ==0: # LCS가 입력 단어랑 같은 경우가 있음
                temp.append(lcs)

            index = find_list_index(LCS_list,lcs) # index = LCS_list의 번지수가 temp_list의 번지수로 매칭됨
            if index == -1: # LCS가 없으면 추가
                temp_list.append(temp)
                LCS_list.append(lcs)
            else:   # LCS가 있으면 중복되지 않게 추가
                for i in range(len(temp)):
                    if temp_list[index].count(temp[i]) == 0:
                        temp_list[index].append(temp[i])

    # 최종 결과는 유사도 비교 결과 리스트의 길이가 2 or 4이상인 경우
    for i in temp_list:
        if len(i) > 3 or len(i) == 2:
            result_list.append(i)

    return result_list if len(result_list) != 0 else [['-1']]

def make_similar_list_into_csv():
    result=[]
    data = pd.read_csv('./wv_csv/similar_word_7.csv', sep=',') # Etri는 하루에 5천번 요청가능 // csv로 하루 가능량 많큼 분할 // similar_word_"i".csv <- i를 바꿔가며 요청
    csv = data.values.tolist()

    csv_list=[]
    for csv_row in csv:
        csv_row = list(filter(lambda v: v==v, csv_row)) # 공백 item 제거
        if len(csv_row) >1:
            csv_list.append(csv_row)

    cnt=0
    for i in csv_list:
        similar_list = find_similar_list(i)

        print(similar_list)
        for j in similar_list:
            print(j)
            if j != ['-1']:
                result.append(j)
        print(cnt)
        cnt += 1
        if cnt > 111 : break

    df = pd.DataFrame(result)
    df.to_csv('./result_csv/result.csv', mode='a', header=False,index=False) # result.csv에 추가 됨

make_similar_list_into_csv()



