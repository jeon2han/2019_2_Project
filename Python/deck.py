import pandas as pd
import random

def make_deck():
    data = pd.read_csv('./result_csv/result.csv', sep=',')
    csv = data.values.tolist()

    csv_list = []
    for csv_row in csv:
        csv_row = list(filter(lambda v: v==v, csv_row))  # 공백 item 제거
        if len(csv_row) > 1:
            csv_list.append(csv_row)
    print(csv_list)

    cnt = 0; # 단어 개수
    deck_count=0; # 만들어진 덱개수
    random_index_list=[] # 인덱스 중복 확인 리스트
    deck=[] # 최종 덱
    while True:
        index = random.randrange(0,10000)%len(csv_list)
        if random_index_list.count(index) !=0:continue # 인덱스 중복 확인
        random_index_list.append(index)

        size = len(csv_list[index])
        if size%2 != 0: size-=1 # 단어수가 홀수인지 짝수인지 확인하여 홀수면 -1

        result=[index]
        temp = csv_list[index]
        for i in range(0,size):
            result.append(temp[i])
            cnt+=1
            if cnt==16:break
        deck.append(result)

        if cnt == 16: # 덱의 단어수가 16개이면 저장 하고 초기화
            df = pd.DataFrame(deck)
            df.to_csv('./deck_csv/deck_'+ str(deck_count) +'.csv', mode='a', header=False,index=False)
            deck=[]
            cnt=0;
            deck_count+=1
            random_index_list=[]

        if deck_count ==30 : break

make_deck()