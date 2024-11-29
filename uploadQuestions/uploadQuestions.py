import requests

topics = ["Var", "InputOutput", "CountPlusPlus", "Arithmetic", "Logical", "for", "while", "dowhile", "if", "array", "functions", "classes", "recursion"]

def upload(topic):
    f = open(topic + "CSV.csv", "r")
    text = f.read()
    lines = text.split('\n')
    print(lines)
    for line in lines:
        blocks = line.split('$,')
        print(blocks)
        url = 'http://127.0.0.1:5000/api/Questions/Add'
        myobj = {'question': blocks[0], 'options': [blocks[1], blocks[2], blocks[3], blocks[4]], 'answer': blocks[5], 'explanation': blocks[6], 'topic': topic}
        x = requests.post(url, json = myobj)
    print(topic + ' DONE')


if __name__ == '__main__':
    for topic in topics:
        upload(topic)
