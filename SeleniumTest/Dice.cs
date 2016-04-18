using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CodeIq {
    class ProgramDice {
        static void MainDice(string[] args) {
            int counterMax = 1000000;
            var tSw = new System.Diagnostics.Stopwatch();
            tSw.Start();

            String inputLine;
            MatchCollection Matches;
            string pattern = @"(G|C|P)";
            int range = 0;
            DiceOdds2 odds;

            for (; (inputLine = Console.ReadLine()) != null;) { // input null?
                range = int.Parse(inputLine);
                Console.WriteLine(new DiceOdds2(range).RelevantThresh);


                System.Diagnostics.Debug.Print( " : " + tSw.Elapsed.ToString());
            }
        }
        public class DiceOdds2 {
            public DiceOdds2(int aRange) {
                range = aRange;
                ConvergentThresh = (long)Math.Pow(10.0, 5);
                expectation = new Dictionary<int, long>();


                for (int count = 1; count <= range; count++) {
                    expectation.Add(count, 0);
                }
                rnd = new Random();
                SimulateDices();
            }

            private Random rnd;
            private int range;
            private Dictionary<int, long> expectation;
            public long ConvergentThresh;

            private void SimulateDices() {
                int value;
                int retry;
                long gross = 0;
                for (int thresh = 1; thresh <= range; thresh++) {
                    for (int trials = 0; trials < ConvergentThresh; trials++) {
                        retry = 0;
                        gross = 0;
                        for (;;) {
                            value = rnd.Next(1, range + 1);
                            if (value >= thresh) {
                                break;
                            }
                            retry++;
                        }
                        //System.Diagnostics.Debug.WriteLine(thresh + ": " + value + " / " + retry);
                        gross = value - retry;
                        expectation[thresh] += gross;
                    }
                    expectation[thresh] /= (long)ConvergentThresh / 1000;
                    System.Diagnostics.Debug.WriteLine(thresh + ": " + expectation[thresh] + " ; " + gross + " / " + ConvergentThresh);
                }
            }
            public int RelevantThresh {
                get {
                    int resultThresh = 0;
                    long result = 0;
                    long tmpResult;
                    foreach (var value in expectation) {
                        tmpResult = value.Value;
                        if (result < tmpResult) {
                            result = tmpResult;
                            resultThresh = value.Key;
                        }
                    }
                    return resultThresh;
                }
            }
        }

        // モンテカルロ法には敵いません。ってか、誤差大きすぎて・・致命的問題がありそう・・
        public class DiceOdds {
            public DiceOdds(int aRange) {
                range = aRange;
                ConvergentThresh = (decimal)Math.Pow(10.0, -7);
                expectation = new Dictionary<int, decimal>();
                decimal previousValue = 0;

                for (int count = (int)aRange / 2; count <= range; count++) {  // /2 省略用
                    decimal current = GetOdds(range, count, 0);
                    expectation.Add(count, current);
                    System.Diagnostics.Debug.WriteLine("count " + count + " : " + current);
                    if (previousValue > current) { break; }
                    previousValue = current;
                }
            }


            private int range;
            private Dictionary<int, decimal> expectation;
            public int RelevantThresh {
                get {
                    int resultThresh = 0;
                    decimal result = 0;
                    decimal tmpResult;
                    foreach (var value in expectation) {
                        //System.Diagnostics.Debug.WriteLine("value " + value );
                        tmpResult = Math.Round(value.Value, 3);
                        if (result < tmpResult) {
                            result = tmpResult;
                            resultThresh = value.Key;
                        }
                    }
                    return resultThresh;
                }

            }
            public decimal ConvergentThresh;

            private decimal GetOdds(decimal aInverseRate, int aThresh, int aDepth) {
                decimal result = 0;
                decimal neumerator = 0;
                int count;
                for (count = aThresh; count <= range; count++) {
                    neumerator += count - aDepth;
                }
                result = neumerator / aInverseRate;
                //System.Diagnostics.Debug.WriteLine( "result(" + aDepth + "): " + result );
                aDepth++;
                aInverseRate *= range;
                if (result > ConvergentThresh) {    // 極小になっていった場合の演算停止判断
                    for (int surplusCount = 1; surplusCount < aThresh; surplusCount++) {
                        result += GetOdds(aInverseRate, aThresh, aDepth);
                    }
                } else {    // 近似：停止せず継続した場合の損失累積が嵩むはずであり、その近似をしないと誤差となる。
                    neumerator = 0;
                    for (int surplusCount = 1; surplusCount < aThresh; surplusCount++) {
                        neumerator += surplusCount - aDepth;
                    }
                    result += neumerator / aInverseRate;
                    //System.Diagnostics.Debug.WriteLine(result + " rate, " + aDepth);
                }
                return result;
            }
        }
    }
}