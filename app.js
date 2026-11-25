"use strict";

// ==========================================
// 🎲 決定的乱数（シード付き）
// ==========================================
// 🔑 文字列をハッシュ化する（FNV-1a）
function hashString(str) {
let h = 0x811c9dc5;
for (let i = 0; i < str.length; i++) {
h ^= str.charCodeAt(i);
h = Math.imul(h, 0x01000193);
}
return h >>> 0;
}

// 🎲 mulberry32：シードから再現可能な乱数を返す関数を作る
function mulberry32(seed) {
let a = seed >>> 0;
return function () {
a = (a + 0x6d2b79f5) | 0;
let t = Math.imul(a ^ (a >>> 15), 1 | a);
t = (t + Math.imul(t ^ (t >>> 7), 61 | t)) ^ t;
return ((t ^ (t >>> 14)) >>> 0) / 4294967296;
};
}

function todayString() {
const d = new Date();
const y = d.getFullYear();
const m = String(d.getMonth() + 1).padStart(2, "0");
const day = String(d.getDate()).padStart(2, "0");
return `${y}-${m}-${day}`;
}

// ==========================================
// 📋 診断データ
// ==========================================
const THEMES = [
{
id: "weather",
label: "天気",
question: "あなたを天気に例えると",
results: [
{ title: "快晴", desc: "雲ひとつない青空のような人。いるだけで周りの空気が明るくなります。\n今日はその明るさが誰かの一日を救うはず。笑顔を惜しまずに過ごしてみてください。"},
{ title: "晴れのち笑顔", desc: "基本は穏やかな晴れ模様、ときどき見せる笑顔が破壊力抜群の人。\n今日は小さな親切が大きく返ってくる予感。いつもの道で寄り道してみるのもおすすめです。"},
{ title: "曇り", desc: "落ち着いていて思慮深い、曇り空タイプ。派手さはないけれど、長く一緒にいると安心感がすごい人です。\n今日は焦らずじっくり考えることが正解。答えは急がなくて大丈夫です。"},
{ title: "小雨", desc: "しっとりと優しい小雨のような人。乾燥した心にそっと潤いを届けます。\n今日は誰かの話を聞く側に回ると良い日。あなたの相づちが相手の支えになります。"},
{ title: "豪雨", desc: "感情の振れ幅が大きい豪雨タイプ。やるときは一気に全力で、周りを巻き込むエネルギーがあります。\n今日は勢いに任せて進むのが吉。ただし言い過ぎには少しだけ注意を。"},
{ title: "雷", desc: "一瞬で場の空気を変える雷タイプ。ひらめきと瞬発力が武器で、退屈とは無縁の人です。\n今日は思いついたアイデアをすぐ口に出してみて。それが突破口になります。"},
{ title: "雪", desc: "静かで凛とした雪タイプ。物静かだけれど、積もれば景色を一変させる存在感があります。\n今日は一人の時間が力をくれる日。好きなことに没頭してみてください。"},
{ title: "虹", desc: "雨上がりに現れる虹のような人。つらい時期を乗り越えた先で、希望を見せるのが得意です。\n今日は誰かの「よかった」のきっかけになれる日。前向きな言葉を選んでみて。"},
],
},
{
id: "color",
label: "色",
question: "あなたのイメージカラーは",
results: [
{ title: "情熱の赤", desc: "行動力と熱量で周りを引っ張る赤。決めたら迷わず突き進む姿が人の心を動かします。\n今日は「やる」と宣言すると運気が上がる日。小さな一歩でも先に進みましょう。"},
{ title: "誠実な青", desc: "信頼と落ち着きの青。約束を守り、コツコツ積み上げる姿勢が評価されています。\n今日は整理整頓がツキを呼ぶ日。机の上と頭の中をすっきりさせてみて。"},
{ title: "癒やしの緑", desc: "自然体で人をほっとさせる緑。争いを好まず、場の調和を大切にする平和主義者です。\n今日は深呼吸が味方になる日。疲れたら外の空気を吸いに行きましょう。"},
{ title: "元気な黄色", desc: "好奇心と笑いの黄色。新しいものにすぐ飛びつくフットワークの軽さが魅力です。\n今日は気になったことを調べるだけでも収穫あり。メモを取るのがおすすめです。"},
{ title: "神秘の紫", desc: "感性豊かでミステリアスな紫。人と違う視点を持っていて、創作や企画で光ります。\n今日は直感を信じてよい日。理由は後からついてきます。"},
{ title: "優しいピンク", desc: "思いやりと柔らかさのピンク。相手の気持ちを先回りして考えられる人です。\n今日は「ありがとう」を多めに伝えてみて。関係が一段と温かくなります。"},
{ title: "純粋な白", desc: "まっさらで誠実な白。ごまかしが苦手で、正面から向き合う姿勢が信頼を集めます。\n今日は新しいことを始めるのに良い日。小さな習慣からどうぞ。"},
{ title: "頼れる黒", desc: "芯の強さと安定感の黒。多くを語らなくても、どっしりした存在感があります。\n今日は引き受けるより「見守る」が正解の場面も。余裕が魅力になります。"},
],
},
{
id: "animal",
label: "動物",
question: "あなたを動物に例えると",
results: [
{ title: "猫", desc: "マイペースで自由な猫タイプ。気分屋に見えて、実は周りをよく見ています。\n今日は無理に合わせなくて大丈夫。自分の心地よさを優先してみてください。"},
{ title: "犬", desc: "素直で人懐っこい犬タイプ。好きな人や物事には全力でまっすぐ向かっていきます。\n今日は素直な気持ちを伝えると良い日。遠慮はほどほどに。"},
{ title: "狼", desc: "一匹狼に見えて仲間思いな狼タイプ。信じた相手のためなら力を尽くします。\n今日はチームで動くと成果が出やすい日。役割分担がカギです。"},
{ title: "狐", desc: "機転が利く狐タイプ。状況を読むのが速く、ピンチをチャンスに変えるのが上手です。\n今日は先回りした準備が効く日。ひと手間かけてみましょう。"},
{ title: "兎", desc: "穏やかで警戒心も強い兎タイプ。慎重に様子を見てから、跳ぶときは大きく跳びます。\n今日は情報収集の日。決断は材料がそろってからで十分です。"},
{ title: "熊", desc: "どっしり構えた熊タイプ。普段は温厚、ここぞというときの集中力がすごい人です。\n今日は体力温存がテーマ。しっかり食べて、しっかり休みましょう。"},
{ title: "鷹", desc: "高い視点で全体を見渡す鷹タイプ。細部より大局を読むのが得意です。\n今日は俯瞰して考えることが突破口に。目先のことから一歩引いてみて。"},
{ title: "亀", desc: "着実に進む亀タイプ。速さでは負けても、続ける力では誰にも負けません。\n今日は昨日の続きを淡々と進めるのが吉。積み重ねは裏切りません。"},
],
},
{
id: "station",
label: "駅",
question: "あなたを駅に例えると",
results: [
{ title: "高崎駅", desc: "いろいろな路線が集まる結節点・高崎駅タイプ。人と人をつなぐ場所に自然といる人です。\n今日は誰かを紹介したり橋渡しをしたりすると、思わぬ広がりが生まれます。"},
{ title: "大宮駅", desc: "大きくて頼れるターミナル・大宮駅タイプ。たくさんの行き先を受け止める懐の深さがあります。\n今日は抱え込みすぎに注意。乗り換え案内のように、人に任せるのも大事です。"},
{ title: "東京駅", desc: "堂々とした中心・東京駅タイプ。責任ある立場でも物おじしない安定感があります。\n今日は堂々としていることが正解。迷いを見せない姿勢が信頼を呼びます。"},
{ title: "新宿駅", desc: "エネルギッシュで多面的な新宿駅タイプ。いろいろな顔を持ち、どこへでも行ける柔軟さがあります。\n今日は寄り道が収穫になる日。いつもと違う出口から出てみましょう。"},
{ title: "池袋駅", desc: "親しみやすくて活気のある池袋駅タイプ。サブカルから日常まで、守備範囲が広い人です。\n今日は好きなものの話で盛り上がれる日。趣味の共有が縁を広げます。"},
{ title: "秋葉原駅", desc: "好きを極める秋葉原駅タイプ。興味のある分野への没入感が周りを驚かせます。\n今日は専門知識が役に立つ場面がありそう。遠慮なく披露してみて。"},
{ title: "横浜駅", desc: "洗練されていて開放的な横浜駅タイプ。新しい風を取り入れるのが上手な人です。\n今日は身だしなみを少し整えると運気アップ。第一印象がものを言います。"},
{ title: "上野駅", desc: "人情と歴史のある上野駅タイプ。古いものも新しいものも大切にできるバランス感覚があります。\n今日は懐かしい連絡が吉報を運ぶかも。返信は早めがおすすめです。"},
],
},
{
id: "food",
label: "食べ物",
question: "あなたを食べ物に例えると",
results: [
{ title: "ラーメン", desc: "熱々で個性派なラーメンタイプ。こだわりが強く、自分の「これだ」という味を持っています。\n今日はこだわりを貫くことが評価される日。中途半端に妥協しないで。"},
{ title: "寿司", desc: "素材の良さで勝負する寿司タイプ。飾らなくても確かな実力がある、職人気質の人です。\n今日は丁寧な仕事が光る日。一つひとつ確実に仕上げていきましょう。"},
{ title: "カレー", desc: "スパイスの効いたカレータイプ。一見穏やかでも、内側に刺激と深みを秘めています。\n今日はいつもと違う選択が面白い展開を呼びます。辛さ増しでどうぞ。"},
{ title: "パンケーキ", desc: "ふわふわ甘いパンケーキタイプ。一緒にいる人をほっと幸せな気持ちにさせます。\n今日は自分へのご褒美も忘れずに。甘いものが元気をくれます。"},
{ title: "焼肉", desc: "豪快でエネルギッシュな焼肉タイプ。場を盛り上げるムードメーカーです。\n今日はみんなで集まる場面にツキあり。幹事役も悪くありません。"},
{ title: "そば", desc: "素朴で凛としたそばタイプ。無駄をそぎ落としたシンプルさに美学があります。\n今日は「減らす」ことが正解の日。持ち物も予定も軽くしてみましょう。"},
{ title: "プリン", desc: "つるんと可愛いプリンタイプ。柔らかそうに見えて、芯にはほろ苦いカラメルのような大人っぽさも。\n今日はギャップが魅力になる日。意外な一面を見せてみて。"},
{ title: "おにぎり", desc: "ほっとする定番・おにぎりタイプ。特別じゃなくても、なくてはならない存在です。\n今日は基本を大切に。当たり前のことを丁寧にやる人が一番強いです。"},
],
},
{
id: "rpg",
label: "RPGジョブ",
question: "あなたのRPGジョブは",
results: [
{ title: "勇者", desc: "困難から逃げない勇者タイプ。責任感が強く、みんなの期待を背負える人です。\n今日は先頭に立つ場面がありそう。迷ったら「やる」を選んでみてください。"},
{ title: "魔法使い", desc: "知識と発想で戦う魔法使いタイプ。頭の回転が速く、難しい問題ほど燃えます。\n今日は調べものや勉強にツキあり。新しい呪文（知識）を覚える日です。"},
{ title: "戦士", desc: "体力と根性の戦士タイプ。地道な努力と継続力で信頼を勝ち取る人です。\n今日は筋の通った行動が評価されます。正面突破が吉。"},
{ title: "僧侶", desc: "回復と支援の僧侶タイプ。周りの疲れにいち早く気づく、優しい人です。\n今日は「大丈夫？」の一言が誰かを救います。ただし自分の回復も忘れずに。"},
{ title: "盗賊", desc: "身軽で機敏な盗賊タイプ。チャンスを見つける嗅覚と、素早い判断が武器です。\n今日はスピード勝負の場面にツキあり。先手必勝で動きましょう。"},
{ title: "賢者", desc: "経験と知恵の賢者タイプ。物事の本質を見抜き、的確な助言ができる人です。\n今日は相談されることが増えそう。答えより「考え方」を渡すと喜ばれます。"},
{ title: "踊り子", desc: "場を華やかにする踊り子タイプ。表現力と魅力で人を惹きつけます。\n今日は見せる・聞かせる場面にツキあり。少し大げさなくらいが丁度いいです。"},
{ title: "商人", desc: "現実感覚に優れた商人タイプ。損得を見極め、資源をうまく回すのが得意です。\n今日はお金や時間の使い方を見直すと収穫あり。無駄の発見が利益になります。"},
],
},
];

// ==========================================
// 🖥️ UI
// ==========================================
const nameInput = document.getElementById("nameInput");
const chipsContainer = document.getElementById("themeChips");
const diagnoseBtn = document.getElementById("diagnoseBtn");
const errorMsg = document.getElementById("errorMsg");
const resultCard = document.getElementById("resultCard");
const resultTheme = document.getElementById("resultTheme");
const resultTitle = document.getElementById("resultTitle");
const resultFor = document.getElementById("resultFor");
const resultDesc = document.getElementById("resultDesc");
const downloadBtn = document.getElementById("downloadBtn");
const shareLink = document.getElementById("shareLink");
const canvas = document.getElementById("cardCanvas");

let selectedThemeId = THEMES[0].id;
let currentResult = null; // { name, theme, result}

THEMES.forEach((theme, i) => {
const btn = document.createElement("button");
btn.type = "button";
btn.className = "chip";
btn.textContent = theme.label;
btn.setAttribute("role", "tab");
btn.setAttribute("aria-selected", i === 0? "true": "false");
btn.addEventListener("click", () => {
selectedThemeId = theme.id;
chipsContainer.querySelectorAll(".chip").forEach((c) =>
c.setAttribute("aria-selected", c === btn? "true": "false")
);
});
chipsContainer.appendChild(btn);
});

function diagnose() {
const name = nameInput.value.trim();
if (!name) {
errorMsg.hidden = false;
resultCard.hidden = true;
return;
}
errorMsg.hidden = true;
const theme = THEMES.find((t) => t.id === selectedThemeId);
const seed = hashString(`${name}|${todayString()}|${theme.id}`);
const rand = mulberry32(seed);
const result = theme.results[Math.floor(rand() * theme.results.length)];
currentResult = { name, theme, result};

resultTheme.textContent = theme.question;
resultTitle.textContent = result.title;
resultFor.textContent = `${name} さんの診断結果（${todayString()}）`;
resultDesc.textContent = result.desc;
resultCard.hidden = false;

const shareText = `${name}さんを${theme.label}に例えると「${result.title}」でした！ #診断メーカー`;
shareLink.href =
"https://twitter.com/intent/tweet?text=" + encodeURIComponent(shareText);
resultCard.scrollIntoView({ behavior: "smooth", block: "start"});
}

diagnoseBtn.addEventListener("click", diagnose);
nameInput.addEventListener("keydown", (e) => {
if (e.key === "Enter") diagnose();
});

// ==========================================
// 🖼️ 結果カード（Canvas 1200×630）
// ==========================================
function wrapText(ctx, text, maxWidth) {
const lines = [];
for (const paragraph of text.split("\n")) {
let line = "";
for (const ch of paragraph) {
const test = line + ch;
if (ctx.measureText(test).width > maxWidth && line.length > 0) {
lines.push(line);
line = ch;
} else {
line = test;
}
}
lines.push(line);
}
return lines;
}

function drawCard() {
if (!currentResult) return;
const { name, theme, result} = currentResult;
const ctx = canvas.getContext("2d");
const W = 1200, H = 630;

const grad = ctx.createLinearGradient(0, 0, W, H);
grad.addColorStop(0, "#4f46e5");
grad.addColorStop(1, "#7c3aed");
ctx.fillStyle = grad;
ctx.fillRect(0, 0, W, H);

ctx.fillStyle = "#ffffff";
ctx.fillRect(60, 60, W - 120, H - 120);

ctx.textAlign = "center";
ctx.fillStyle = "#5c6370";
ctx.font = "28px sans-serif";
ctx.fillText(theme.question, W / 2, 150);

ctx.fillStyle = "#4f46e5";
ctx.font = "bold 96px sans-serif";
ctx.fillText(result.title, W / 2, 260);

ctx.fillStyle = "#1f2430";
ctx.font = "30px sans-serif";
ctx.fillText(`${name} さんの診断結果`, W / 2, 325);

ctx.font = "27px sans-serif";
ctx.fillStyle = "#333a47";
const lines = wrapText(ctx, result.desc, W - 240);
lines.slice(0, 5).forEach((line, i) => {
ctx.fillText(line, W / 2, 385 + i * 40);
});

ctx.fillStyle = "#8a90a0";
ctx.font = "22px sans-serif";
ctx.fillText(`診断メーカー ・ ${todayString()}`, W / 2, H - 85);
}

downloadBtn.addEventListener("click", () => {
if (!currentResult) return;
drawCard();
const a = document.createElement("a");
a.download = `shindan-${currentResult.theme.id}-${todayString()}.png`;
a.href = canvas.toDataURL("image/png");
a.click();
});

// ------------------------------------------
// 🌙 テーマ切り替え（ダーク既定＋ライト切替）
// ------------------------------------------
(function initTheme() {
  var root = document.documentElement;
  if (!root || !root.getAttribute || !root.setAttribute) return;
  function apply(theme) {
    root.setAttribute("data-theme", theme);
    var btn = document.getElementById("themeToggle");
    if (btn) btn.textContent = theme === "light" ? "☀️" : "🌙";
  }
  var current = root.getAttribute("data-theme");
  if (current !== "light" && current !== "dark") {
    try { current = localStorage.getItem("shindanya-theme"); } catch (e) { current = null; }
  }
  apply(current === "light" ? "light" : "dark");
  var btn = document.getElementById("themeToggle");
  if (btn && btn.addEventListener) {
    btn.addEventListener("click", function () {
      var next = root.getAttribute("data-theme") === "light" ? "dark" : "light";
      apply(next);
      try { localStorage.setItem("shindanya-theme", next); } catch (e) { /* 保存できなくても切り替え自体は有効 */ }
    });
  }
})();
