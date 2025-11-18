using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VPT_Login.Models;

namespace VPT_Login.Libs
{
    public class AutoXuQue
    {
        private AutoFeatures mAuto;
        private DataModel mChar;

        public AutoXuQue(AutoFeatures auto, DataModel dataModel)
        {
            mAuto = auto;
            mChar = dataModel;
        }

        public void AutoLatThe()
        {
            mAuto.WriteStatus("Bắt đầu \"Lật thẻ\" ...");
            mAuto.CloseAllDialog();
            Thread.Sleep(Constant.TimeMediumShort);
            int check = 0;
            while (!mAuto.FindImageByGroup("xu_que", "latTheCheck") && check < 5)
            {
                // Mở xủ quẻ
                while (!mAuto.FindImageByGroup("xu_que", "xuQueTui"))
                {
                    // Kéo quick feature list lên trên cùng
                    while (mAuto.FindImageByGroup("global", "quickFeatureListUpArrow") &&
                        !mAuto.FindImageByGroup("global", "quickFeatureXuQue"))
                    {
                        mAuto.ClickImageByGroup("global", "quickFeatureListUpArrow");
                        Thread.Sleep(Constant.TimeShort);
                    }

                    // Kéo quick feature list xuống để tìm xủ quẻ
                    while (!mAuto.FindImageByGroup("global", "quickFeatureXuQue") && mAuto.FindImageByGroup("global", "quickFeatureListDownArrow"))
                    {
                        mAuto.ClickImageByGroup("global", "quickFeatureListDownArrow");
                        Thread.Sleep(Constant.TimeShort);
                    }

                    // Mở xủ quẻ
                    mAuto.ClickImageByGroup("global", "quickFeatureXuQue");
                    Thread.Sleep(Constant.TimeMediumShort);

                    //Mở lật thẻ
                    if (mAuto.FindImageByGroup("xu_que", "iconLatThe"))
                    {
                        mAuto.WriteStatus("Mở lật thẻ");
                        mAuto.ClickImageByGroup("xu_que", "iconLatThe");
                        Thread.Sleep(Constant.TimeMedium);
                        break;
                    }
                }

                check++;
            }
            check = 0;
            mAuto.WriteStatus("Đã mở");

            while (check < 6)
            {
                mAuto.ClickImageByGroup("xu_que", "latThe");
                Thread.Sleep(Constant.TimeMediumShort);

                List<KeyValuePair<int, int>> cardCounts = new List<KeyValuePair<int, int>>();
                for (int i = 0; i < 6; i++)
                {
                    var found = mAuto.FindImages($"/xu_que/latthe_so_{i + 1}.png");
                    cardCounts.Add(new KeyValuePair<int, int>(i + 1, found?.Count() ?? 0));
                }
                if (cardCounts.Sum(kv => kv.Value) == 0)
                {
                    break;
                }

                if (mAuto.FindImageByGroup("xu_que", "latthe_ketqua_1", percent: .9) ||
                    mAuto.FindImageByGroup("xu_que", "latthe_ketqua_2", percent: .9) ||
                    mAuto.FindImageByGroup("xu_que", "latthe_ketqua_3", percent: .9) ||
                    mAuto.FindImageByGroup("xu_que", "latthe_ketqua_4", percent: .9))
                {

                    var keepCards = getCardValuesToKeep(cardCounts);
                    int cardsToChange = 5 - keepCards.Sum(kv => kv.Value);
                    int count = 0;

                    for (int i = 0; i < 6 && count < cardsToChange; i++)
                    {
                        if (mAuto.FindImageByGroup("xu_que", "latthe_so_" + (i + 1)) &&
                            !keepCards.Select(x => x.Key).Contains(i + 1))
                        {
                            mAuto.ClickImageByGroup("xu_que", "latthe_so_" + (i + 1));
                            Thread.Sleep(700);
                            count++;
                        }
                    }

                    if (count > 0)
                    {
                        mAuto.ClickImageByGroup("xu_que", "lat_thedoi");
                        Thread.Sleep(Constant.TimeMediumShort);
                    }
                }
                mAuto.ClickImageByGroup("xu_que", "nhanLatThe");
                Thread.Sleep(Constant.TimeShort);

                mAuto.ClickImageByGroup("xu_que", "latTheCo", false, true);
                mAuto.ClickImageByGroup("xu_que", "latTheCo2", false, true);
                Thread.Sleep(Constant.TimeMediumShort);
                check++;

                if (mAuto.FindImageByGroup("xu_que", "hetLuot", percent: .9))
                {
                    break;
                }
            }

            Thread.Sleep(Constant.TimeShort);
            mAuto.CloseAllDialog();
            mChar.LatBaiXong.Value = true;
            mAuto.WriteStatus("Hoàn thành \"Lật thẻ\" ...");
        }

        private List<KeyValuePair<int, int>> getCardValuesToKeep(List<KeyValuePair<int, int>> cardCounts)
        {
            // (bỏ qua nếu bạn luôn đảm bảo cardCounts chứa 1..6)
            var countsDict = cardCounts.ToDictionary(kv => kv.Key, kv => kv.Value);

            var sorted = cardCounts
                .Where(kv => kv.Value > 0)
                .OrderByDescending(kv => kv.Value)   // ưu tiên số lượng lớn
                .ToList();

            if (sorted.Count == 1)
            {
                return new List<KeyValuePair<int, int>> { sorted[0] };
            }

            // 3 đồng
            if (sorted.Count > 0 && sorted[0].Value == 3)
            {
                // trả về KeyValuePair của giá trị có 3 lá (giữ key + count hiện có)
                return new List<KeyValuePair<int, int>> { sorted[0] };
            }

            // 2 đôi (hai giá trị có count == 2) -> giữ đôi lớn hơn (pairs[0] do sorted sắp)
            var pairs = sorted.Where(kv => kv.Value == 2).ToList();
            if (pairs.Count == 2)
            {
                return pairs;
            }

            // 1 đôi -> kiểm tra chuỗi 4 liên tiếp
            if (sorted.Count > 0 && sorted[0].Value == 2)
            {
                // Các chuỗi 4 giá trị liên tiếp cần kiểm tra
                List<int[]> possibleRuns = new List<int[]>
    {
        new int[] {1, 2, 3, 4},
        new int[] {2, 3, 4, 5},
        new int[] {3, 4, 5, 6}
    };

                foreach (var run in possibleRuns)
                {
                    // Kiểm tra từng giá trị trong chuỗi: phải tồn tại (count >= 1)
                    bool allExist = run.All(v => countsDict.ContainsKey(v) && countsDict[v] > 0);
                    if (allExist)
                    {
                        // Trả về toàn bộ lá của chuỗi liên tiếp này
                        return run.Select(v => new KeyValuePair<int, int>(v, countsDict[v])).ToList();
                    }
                }

                // Nếu không có chuỗi liên tiếp đủ điều kiện → chỉ giữ lại đôi
                return new List<KeyValuePair<int, int>> { pairs[0] };
            }

            // không gì => úp hết
            return new List<KeyValuePair<int, int>>();
        }

        public bool XuQue()
        {
            mAuto.WriteStatus("Bắt đầu \"Xủ Quẻ\" ...");
            mAuto.CloseAllDialog();

            // Mở xủ quẻ
            while (!mAuto.FindImageByGroup("xu_que", "xuQueTui"))
            {
                // Kéo quick feature list lên trên cùng
                while (mAuto.FindImageByGroup("global", "quickFeatureListUpArrow") &&
                    !mAuto.FindImageByGroup("global", "quickFeatureXuQue"))
                {
                    mAuto.ClickImageByGroup("global", "quickFeatureListUpArrow");
                    Thread.Sleep(Constant.TimeShort);
                }

                // Kéo quick feature list xuống để tìm xủ quẻ
                while (!mAuto.FindImageByGroup("global", "quickFeatureXuQue") && mAuto.FindImageByGroup("global", "quickFeatureListDownArrow"))
                {
                    mAuto.ClickImageByGroup("global", "quickFeatureListDownArrow");
                    Thread.Sleep(Constant.TimeShort);
                }

                // Mở xủ quẻ
                mAuto.ClickImageByGroup("global", "quickFeatureXuQue");
                Thread.Sleep(Constant.TimeMedium);
            }

            while (true)
            {
                // Phân giải nếu thấy
                //while (mAuto.FindImageByGroup("xu_que", "xuQueTrang", percent: .9) ||
                //    mAuto.FindImageByGroup("xu_que", "xuQueLuc", percent: .9) ||
                //    mAuto.FindImageByGroup("xu_que", "xuQueLam", percent: .9))
                //{
                //    if (mAuto.FindImageByGroup("xu_que", "xuQueTrang", percent: .9))
                //        mAuto.ClickImageByGroup("xu_que", "xuQueTrang", x: 22, y: 0, percent: .9);
                //    if (mAuto.FindImageByGroup("xu_que", "xuQueLuc", percent: .9))
                //        mAuto.ClickImageByGroup("xu_que", "xuQueLuc", x: 22, y: 0, percent: .9);
                //    if (mAuto.FindImageByGroup("xu_que", "xuQueLam", percent: .9))
                //    {
                //        mAuto.ClickImageByGroup("xu_que", "xuQueLam", x: 22, y: 0, percent: .9);

                //    }
                //    while (mAuto.FindImageByGroup("xu_que", "xuQueXacNhanPhanGiai"))
                //    {
                //        mAuto.ClickImageByGroup("xu_que", "xuQuePhanGiai",false,true);
                //    }
                //}

                while (mAuto.FindImageByGroup("xu_que", "xuQueGiai", false, true, .9))
                {
                    if (mAuto.FindImageByGroup("xu_que", "xuQueGiai", false, true))
                    {
                        mAuto.ClickImageByGroup("xu_que", "xuQueGiai", false, true);

                    }
                    while (mAuto.FindImageByGroup("xu_que", "xuQueXacNhanPhanGiai"))
                    {
                        //mAuto.ClickImageByGroup("xu_que", "xuQuePhanGiai", false, true);
                        mAuto.ClickImageByGroup("xu_que", "latTheCo", false, true);
                        mAuto.ClickImageByGroup("xu_que", "latTheCo2", false, true);
                    }
                }


                // Xủ quẻ :D
                int i = 0;
                while (i < 3)
                {
                    i++;
                    mAuto.ClickImageByGroup("xu_que", "xuQue3Sao");
                    mAuto.ClickImageByGroup("xu_que", "xuQue4Sao");
                    mAuto.ClickImageByGroup("xu_que", "xuQue5Sao");

                    int j = 0;
                    while (j < 2)
                    {
                        j++;
                        mAuto.ClickImageByGroup("xu_que", "xuQue1Sao");
                        mAuto.ClickImageByGroup("xu_que", "xuQue2Sao");
                    }
                }
            }

            return true;
        }
    }

}
