using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFProject.Classes.Comboboxes
{
    public class ComboboxFinder : ComboboxPairs
    {
        private string comboboxKey;

        public ComboboxFinder(string comboboxKey) { this.comboboxKey = comboboxKey; }

        private KeyValuePair<string, string> FindValueByKey(List<KeyValuePair<string, string>> pairs)
        {
            foreach (var pair in pairs) if (pair.Key == this.comboboxKey) return pair;
            return new KeyValuePair<string, string>(null, null);
        }

        public KeyValuePair<string, string> CategoryPair { get => FindValueByKey(pairs: Category); }
        public KeyValuePair<string, string> CashFlowPair { get => FindValueByKey(pairs: CashFlow); }
        public KeyValuePair<string, string> OriginalityPair { get => FindValueByKey(pairs: Originality); }
        public KeyValuePair<string, string> SocialProfitPair { get => FindValueByKey(pairs: SocialProfit); }
    }
}
