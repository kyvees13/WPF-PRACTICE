using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFProject.Classes.Comboboxes
{
    public static class ComboboxPairs
    {
        public static List<KeyValuePair<string, string>> Category = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(key: "Услуги млн. руб.", value: "1"),
                new KeyValuePair<string, string>(key: "Строительные смеси тыс. тонн", value: "2"),
                new KeyValuePair<string, string>(key: "Древесина тыс. куб. м.", value: "3"),
                new KeyValuePair<string, string>(key: "Кирпич и бетонные блоки тыс. шт.", value: "4"),
                new KeyValuePair<string, string>(key: "Транспортные услуги млн. руб.", value: "5"),
                new KeyValuePair<string, string>(key: "Продукты питания млн. руб.", value: "6")
            };
        public static List<KeyValuePair<string, string>> CashFlow = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(key: "0-1", value: "1"),
                new KeyValuePair<string, string>(key: "1.0-10", value: "3"),
                new KeyValuePair<string, string>(key: "10.0-50", value: "5"),
                new KeyValuePair<string, string>(key: "50.0+", value: "6")
            };
        public static List<KeyValuePair<string, string>> Originality = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(key: "Уникальный", value: "5"),
                new KeyValuePair<string, string>(key: "Есть аналоги в РФ", value: "3"),
                new KeyValuePair<string, string>(key: "Есть аналоги в регионе", value: "2"),
            };
        public static List<KeyValuePair<string, string>> SocialProfit = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>(key: "Все население", value: "5"),
                new KeyValuePair<string, string>(key: "Средний класс", value: "3"),
                new KeyValuePair<string, string>(key: "Социальнонезащищенные", value: "3"),
                new KeyValuePair<string, string>(key: "2% населения", value: "1")
            };
    }
}
