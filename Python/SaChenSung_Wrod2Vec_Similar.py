from gensim.models import Word2Vec
import re
import pandas as pd

# 임베딩된 단어 : 358043
# 한글 단어 : 225787
# 4글자 이하 한글 단어 : 212727

# 임베딩된 단어 추출 함수
def extract_wv_embedding_word():
    model = Word2Vec.load('./model/word2vec')

    temp = list(model.wv.vocab.keys())
    embedding_list = []

    # 한글이 아니거나 길이가 5이상인 단어 제외
    korean = re.compile('[^가-힣]+')
    for i in temp:
        word = korean.sub('', i)
        if len(word) != len(i): continue
        if word != "" and len(word) < 5:
            embedding_list.append(word)

    return list(set(embedding_list))


# 임베딩된 단어 csv로 만듬s
# 입력단어 1개, 유사도 비교 결과 단어 9개, 총 10개 단어묶음이 한 row
# csv에 111개의 열로 이루어짐 / 111*45 = 4995 / 하루 api 가능 건수 = 5000
def make_wv_embedding_word_into_csv():
    csv_cnt = 0; # csv 개수 카운트
    cnt = 0 # 111개 카운트

    model = Word2Vec.load('./model/word2vec')
    embedding_word = extract_wv_embedding_word()

    row_111 = [] # 111개의 리스트를 담을 리스트
    for ew in embedding_word:
        similar_result = model.wv.most_similar(ew,topn=9)
        sr_list = [ew] # 10개의 단어로 구성된 리스트
        for sr in similar_result: # sr = (단어,정확도) 튜플 쌍
            if(len(sr[0]) < 5): sr_list.append(sr[0])
        row_111.append(sr_list)
        cnt+=1
        print(csv_cnt,cnt)
        if cnt == 111:
            df = pd.DataFrame(row_111)
            df.to_csv('./wv_csv/similar_word_'+ str(csv_cnt) +'.csv', index=False)
            csv_cnt+=1
            cnt=0
            row_111=[]

make_wv_embedding_word_into_csv()

# model = Word2Vec.load('./model/word2vec')
# print(model.most_similar("술",topn=10))

