# TaiwanBloodServices API

擷取[台灣血液基金會](https://www.blood.org.tw/Internet/main/index.aspx)最新血液庫存量，並組合成 JSON 格式，包含台北、新竹、台中、台南、高雄，範例資料如下：

```json
[
  {
    "type": "A型",
    "status": "庫存量4日以下",
    "location": "台北",
    "lastUpdateTime": "2022年04月29日 08時22分"
  },
  {
    "type": "A型",
    "status": "庫存量4到7日",
    "location": "新竹",
    "lastUpdateTime": "2022年04月29日 08時22分"
  },
  {
    "type": "A型",
    "status": "庫存量4到7日",
    "location": "台中",
    "lastUpdateTime": "2022年04月29日 08時22分"
  },
  {
    "type": "A型",
    "status": "庫存量4到7日",
    "location": "台南",
    "lastUpdateTime": "2022年04月29日 08時22分"
  },
  {
    "type": "A型",
    "status": "庫存量4到7日",
    "location": "高雄",
    "lastUpdateTime": "2022年04月29日 08時22分"
  },
  {
    "type": "B型",
    "status": "庫存量7日以上",
    "location": "台北",
    "lastUpdateTime": "2022年04月29日 08時22分"
  },
  {
    "type": "B型",
    "status": "庫存量4到7日",
    "location": "新竹",
    "lastUpdateTime": "2022年04月29日 08時22分"
  },
  {
    "type": "B型",
    "status": "庫存量4到7日",
    "location": "台中",
    "lastUpdateTime": "2022年04月29日 08時22分"
  }
]
```
