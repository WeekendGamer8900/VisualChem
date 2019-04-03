using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualChem
{
    using System.ComponentModel;
    using System.Text.RegularExpressions;
    using static Helper;
    public class Structure
    {
        static Dictionary<string, Operators> dictOperators =
            Enum.GetValues(typeof(Operators)).Cast<Operators>().ToDictionary((op) => op.ToDString());
        static Dictionary<string, Vowels> dictVowels =
            Enum.GetValues(typeof(Vowels)).Cast<Vowels>().ToDictionary((op) => op.ToDString());
        static Dictionary<string, EngPrefixes> dictEngPrefixes =
            Enum.GetValues(typeof(EngPrefixes)).Cast<EngPrefixes>().ToDictionary((op) => op.ToDString());
        static Dictionary<string, ChemPrefixes> dictChemPrefixes =
            Enum.GetValues(typeof(ChemPrefixes)).Cast<ChemPrefixes>().ToDictionary((op) => op.ToDString());
        static Dictionary<string, FunctionalGps> dictFunctionalGps =
            Enum.GetValues(typeof(FunctionalGps)).Cast<FunctionalGps>().ToDictionary((op) => op.ToDString());
        static Dictionary<string, Bonds> dictBonds =
            Enum.GetValues(typeof(Bonds)).Cast<Bonds>().ToDictionary((op) => op.ToDString());
        static Dictionary<string, Suffixes> dictSuffixes =
            Enum.GetValues(typeof(Suffixes)).Cast<Suffixes>().ToDictionary((op) => op.ToDString());

        static Dictionary<string, object> dictAllFunctionalGps =
        dictOperators.ToDictionary(p => p.Key, p => (object)p.Value)
        .Union(dictVowels.ToDictionary(p => p.Key, p => (object)p.Value))
        .Union(dictEngPrefixes.ToDictionary(p => p.Key, p => (object)p.Value))
        .Union(dictChemPrefixes.ToDictionary(p => p.Key, p => (object)p.Value))
        .Union(dictFunctionalGps.ToDictionary(p => p.Key, p => (object)p.Value)).ToDictionary(s => s.Key, s => s.Value);

        static Dictionary<string, object> dictAllParentChain =
        dictChemPrefixes.ToDictionary(p => p.Key, p => (object)p.Value).ToDictionary(s => s.Key, s => s.Value);

        static Dictionary<string, object> dictAllTail =
        dictOperators.ToDictionary(p => p.Key, p => (object)p.Value)
        .Union(dictVowels.ToDictionary(p => p.Key, p => (object)p.Value))
        .Union(dictEngPrefixes.ToDictionary(p => p.Key, p => (object)p.Value))
        .Union(dictBonds.ToDictionary(p => p.Key, p => (object)p.Value))
        .Union(dictSuffixes.ToDictionary(p => p.Key, p => (object)p.Value)).ToDictionary(s => s.Key, s => s.Value);

        static List<string> allTokens =
        dictOperators.Keys
        .Union(dictVowels.Keys)
        .Union(dictEngPrefixes.Keys)
        .Union(dictChemPrefixes.Keys)
        .Union(dictFunctionalGps.Keys)
        .Union(dictBonds.Keys)
        .Union(dictSuffixes.Keys)
        .OrderByDescending((str) => str.Length).Where((s) => s.Length > 0).ToList();

        static List<string> functionalGpsTokens =
        dictOperators.Keys
        .Union(dictVowels.Keys)
        .Union(dictEngPrefixes.Keys)
        .Union(dictChemPrefixes.Keys)
        .Union(dictFunctionalGps.Keys)
        .OrderByDescending((str) => str.Length).Where((s) => s.Length > 0).ToList();

        static List<string> parentChainTokens =
        dictChemPrefixes.Keys
        .OrderByDescending((str) => str.Length).Where((s) => s.Length > 0).ToList();

        static List<string> tailTokens =
        dictOperators.Keys
        .Union(dictVowels.Keys)
        .Union(dictEngPrefixes.Keys)
        .Union(dictBonds.Keys)
        .Union(dictSuffixes.Keys)
        .OrderByDescending((str) => str.Length).Where((s) => s.Length > 0).ToList();

        public enum Operators
        {
            [Description("")]
            number,
            [Description("-")]
            hyphen,
            [Description(",")]
            comma
        }

        public enum Vowels
        {
            a, e
        }

        public enum EngPrefixes
        {
            di = 2,
            tri = 3,
            tetra = 4,
            penta = 5,
            hexa = 6,
            hepta = 7,
            octa = 8,
        }
        public enum ChemPrefixes
        {
            meth = 1,
            eth = 2,
            prop = 3,
            but = 4,
            pent = 5,
            hex = 6,
            hept = 7,
            oct = 8,
            non = 9,
            dec = 10,
            undec = 11,
            dodec = 12,
            tridec = 13,
            tetradec = 14,
            pentadec = 15,
            hexadec = 16,
            heptadec = 17,
            octadec = 18,
            nonadec = 19,
            icos = 20,
            henicos = 21,
            docos = 22,
            tricos = 23,
            tetracos = 24,
            pentacos = 25,
            hexacos = 26,
            heptacos = 27,
            octacos = 28,
            nonacos = 29,
            triacont = 30,
            hentriacont = 31,
            dotriacont = 32,
            tritriacont = 33,
            tetratriacont = 34,
            pentatriacont = 35,
            hexatriacont = 36,
            heptatriacont = 37,
            octatriacont = 38,
            nonatriacont = 39,
            tetracont = 40,
            hentetracont = 41,
            dotetracont = 42,
            tritetracont = 43,
            tetratetracont = 44,
            pentatetracont = 45,
            hexatetracont = 46,
            heptatetracont = 47,
            octatetracont = 48,
            nonatetracont = 49,
            pentacont = 50,
            henpentacont = 51,
            dopentacont = 52,
            tripentacont = 53,
            tetrapentacont = 54,
            pentapentacont = 55,
            hexapentacont = 56,
            heptapentacont = 57,
            octapentacont = 58,
            nonapentacont = 59,
            hexacont = 60,
            henhexacont = 61,
            dohexacont = 62,
            trihexacont = 63,
            tetrahexacont = 64,
            pentahexacont = 65,
            hexahexacont = 66,
            heptahexacont = 67,
            octahexacont = 68,
            nonahexacont = 69,
            heptacont = 70,
            henheptacont = 71,
            doheptacont = 72,
            triheptacont = 73,
            tetraheptacont = 74,
            pentaheptacont = 75,
            hexaheptacont = 76,
            heptaheptacont = 77,
            octaheptacont = 78,
            nonaheptacont = 79,
            octacont = 80,
            henoctacont = 81,
            dooctacont = 82,
            trioctacont = 83,
            tetraoctacont = 84,
            pentaoctacont = 85,
            hexaoctacont = 86,
            heptaoctacont = 87,
            octaoctacont = 88,
            nonaoctacont = 89,
            nonacont = 90,
            hennonacont = 91,
            dononacont = 92,
            trinonacont = 93,
            tetranonacont = 94,
            pentanonacont = 95,
            hexanonacont = 96,
            heptanonacont = 97,
            octanonacont = 98,
            nonanonacont = 99,
            hect = 100,
            henihect = 101,
            dohect = 102,
            trihect = 103,
            tetrahect = 104,
            pentahect = 105,
            hexahect = 106,
            heptahect = 107,
            octahect = 108,
            nonahect = 109,
            decahect = 110,
            undecahect = 111,
            dodecahect = 112,
            tridecahect = 113,
            tetradecahect = 114,
            pentadecahect = 115,
            hexadecahect = 116,
            heptadecahect = 117,
            octadecahect = 118,
            nonadecahect = 119,
            icosahect = 120,    
        }
        public enum FunctionalGps
        {
            yl, oxy, hydroxy, carboxyl, bromo, chloro, fluoro, oxo, cyclo, phenyl
        }
        public enum Bonds
        {
            an = 1, en = 2, yn = 3
        }
        public enum Suffixes
        {
            [Description("oic acid")]
            oic_acid,
            [Description("carboxylic acid")]
            carboxylic_acid,
            ol,
            oate,
            al,
            one,
            amine
        }

        public class Token
        {
            public object Type;
            public int data;
            public Token(object T, int D)
            {
                Type = T; data = D;
            }
        }

        public class StringExpression
        {
            public List<string> FunctionalGpTokens = new List<string>();
            public List<string> ParentChainTokens = new List<string>();
            public List<string> TailTokens = new List<string>();
        }

        public class TokenizedExpression
        {
            public List<Token> FunctionalGpTokens = new List<Token>();
            public List<Token> ParentChainTokens = new List<Token>();
            public List<Token> TailTokens = new List<Token>();
        }

        public class NameModifier
        {
            public int Locations;
            public object Type;
            public List<NameModifier> Modifiers = new List<NameModifier>();
        }

        public class Instructions
        {
            public List<NameModifier> FunctionalGps = new List<NameModifier>();
            public object ParentChain;
            public List<NameModifier> Tails = new List<NameModifier>();
        }

        public class Molecule
        {
            public List<Node> Nodes = new List<Node>();
            public List<Bond> Bonds = new List<Bond>();

            public Molecule()
            {

            }

            List<string> FindStringTokens(string name, List<string> refTokens)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return new List<string>();
                }
                foreach (string t in refTokens)
                {
                    if (name.StartsWith(t))
                    {
                        List<string> tmp = FindStringTokens(name.Substring(t.Length), refTokens);
                        if (tmp != null)
                        {
                            tmp.Insert(0, t);
                            return tmp;
                        }
                    }
                }
                if (isNumeric(name[0]))
                {
                    int i = 0;
                    string num = GetNumForward(name, ref i);
                    List<string> tmp = FindStringTokens(name.Substring(i + 1), refTokens);
                    if (tmp != null)
                    {
                        tmp.Insert(0, num);
                        return tmp;
                    }
                }
                return null;
            }

            public StringExpression Lexer(string name)
            {
                StringExpression exp = new StringExpression();
                Match m = Regex.Match(name, "^([a-z\\-,0-9()]*?)(?:((?:^|(?<=[a-z)]))(?:" + Enum.GetValues(typeof(ChemPrefixes)).Cast<ChemPrefixes>().Select((op) => op.ToDString()).OrderByDescending((s)=>s.Length).Aggregate((a, b) => a + "|" + b) + ")(?!yl|oxy))([a-z\\-,0-9 ]*?))$");
                exp.FunctionalGpTokens = FindStringTokens(m.Groups[1].Value, functionalGpsTokens);
                exp.ParentChainTokens = FindStringTokens(m.Groups[2].Value, parentChainTokens);
                exp.TailTokens = FindStringTokens(m.Groups[3].Value, tailTokens);
                return exp;
            }

            public TokenizedExpression Tokenize(StringExpression exp)
            {
                TokenizedExpression ret = new TokenizedExpression();
                ret.FunctionalGpTokens = exp.FunctionalGpTokens.Select((str) =>
                {
                    if (Regex.IsMatch(str, @"^\d+$"))
                    {
                        return new Token(Operators.number, int.Parse(str));
                    }
                    else
                    {
                        return new Token(dictAllFunctionalGps[str], 0);
                    }
                }).ToList();
                ret.ParentChainTokens = exp.ParentChainTokens.Select((str) =>
                {
                    return new Token(dictAllParentChain[str], 0);
                }).ToList();
                ret.TailTokens = exp.TailTokens.Select((str) =>
                {
                    if (Regex.IsMatch(str, @"^\d+$"))
                    {
                        return new Token(Operators.number, int.Parse(str));
                    }
                    else
                    {
                        return new Token(dictAllTail[str], 0);
                    }
                }).ToList();
                return ret;
            }

            void OicAcid(Node carbon)
            {
                Node nodeO1 = new Node(Elements.Oxygen);
                Node nodeO2 = new Node(Elements.Oxygen);
                Node nodeH = new Node(Elements.Hydrogen);
                Nodes.Add(nodeO1);
                Nodes.Add(nodeO2);
                Nodes.Add(nodeH);
                Bonds.Add(new Bond(carbon, nodeO2, BondType.Single, Orientation.Horizontal));
                Bonds.Add(new Bond(carbon, nodeO1, BondType.Double, Orientation.Vertical));
                Bonds.Add(new Bond(nodeO2, nodeH, BondType.Single, Orientation.Horizontal));
            }

            void CarboxylicAcid(Node carbon)
            {
                Node nodeC = new Node(Elements.Carbon);
                Node nodeO1 = new Node(Elements.Oxygen);
                Node nodeO2 = new Node(Elements.Oxygen);
                Node nodeH = new Node(Elements.Hydrogen);
                Nodes.Add(nodeC);
                Nodes.Add(nodeO1);
                Nodes.Add(nodeO2);
                Nodes.Add(nodeH);
                Bonds.Add(new Bond(carbon, nodeC, BondType.Single, Orientation.Horizontal));
                Bonds.Add(new Bond(nodeC, nodeO2, BondType.Single, Orientation.Horizontal));
                Bonds.Add(new Bond(nodeC, nodeO1, BondType.Double, Orientation.Vertical));
                Bonds.Add(new Bond(nodeO2, nodeH, BondType.Single, Orientation.Horizontal));
            }

            void Amine(Node carbon)
            {
                Node nodeN = new Node(Elements.Nitrogen);
                Node nodeH1 = new Node(Elements.Hydrogen);
                Node nodeH2 = new Node(Elements.Hydrogen);
                Nodes.Add(nodeN);
                Nodes.Add(nodeH1);
                Nodes.Add(nodeH2);
                Bonds.Add(new Bond(carbon, nodeN, BondType.Single, Orientation.Horizontal));
                Bonds.Add(new Bond(nodeN, nodeH1, BondType.Single, Orientation.Vertical));
                Bonds.Add(new Bond(nodeN, nodeH2, BondType.Single, Orientation.Vertical));
            }

            void Aldehyde(Node carbon)
            {
                Node nodeO = new Node(Elements.Oxygen);
                Node nodeH = new Node(Elements.Hydrogen);
                Nodes.Add(nodeO);
                Nodes.Add(nodeH);
                Bonds.Add(new Bond(carbon, nodeO, BondType.Double, Orientation.Vertical));
                Bonds.Add(new Bond(carbon, nodeH, BondType.Single, Orientation.Horizontal));
            }

            void Alcohol(Node carbon)
            {
                Node nodeO = new Node(Elements.Oxygen);
                Node nodeH = new Node(Elements.Hydrogen);
                Nodes.Add(nodeO);
                Nodes.Add(nodeH);
                Bonds.Add(new Bond(carbon, nodeO, BondType.Single, Orientation.Horizontal));
                Bonds.Add(new Bond(nodeH, nodeO, BondType.Single, Orientation.Horizontal));
            }

            void Ketone(Node carbon)
            {
                Node nodeO = new Node(Elements.Oxygen);
                Nodes.Add(nodeO);
                Bonds.Add(new Bond(carbon, nodeO, BondType.Double, Orientation.Vertical));
            }

            void Bromine(Node carbon)
            {
                Node nodeBr = new Node(Elements.Bromine);
                Nodes.Add(nodeBr);
                Bonds.Add(new Bond(carbon, nodeBr, BondType.Single, Orientation.None));
            }
            void Fluorine(Node carbon)
            {
                Node nodeF = new Node(Elements.Fluorine);
                Nodes.Add(nodeF);
                Bonds.Add(new Bond(carbon, nodeF, BondType.Single, Orientation.None));
            }

            void Chlorine(Node carbon)
            {
                Node nodeBr = new Node(Elements.Chlorine);
                Nodes.Add(nodeBr);
                Bonds.Add(new Bond(carbon, nodeBr, BondType.Single, Orientation.None));
            }

            void Phenyl(Node carbon)
            {
                Node nodeC1 = new Node(Elements.Carbon);
                Node nodeC2 = new Node(Elements.Carbon);
                Node nodeC3 = new Node(Elements.Carbon);
                Node nodeC4 = new Node(Elements.Carbon);
                Node nodeC5 = new Node(Elements.Carbon);
                Node nodeC6 = new Node(Elements.Carbon);
                Nodes.Add(nodeC1);
                Nodes.Add(nodeC2);
                Nodes.Add(nodeC3);
                Nodes.Add(nodeC4);
                Nodes.Add(nodeC5);
                Nodes.Add(nodeC6);
                Bonds.Add(new Bond(carbon, nodeC1, BondType.Single, Orientation.Vertical));
                Bonds.Add(new Bond(nodeC1, nodeC2, BondType.Single, Orientation.None));
                Bonds.Add(new Bond(nodeC2, nodeC3, BondType.Double, Orientation.None));
                Bonds.Add(new Bond(nodeC3, nodeC4, BondType.Single, Orientation.None));
                Bonds.Add(new Bond(nodeC4, nodeC5, BondType.Double, Orientation.None));
                Bonds.Add(new Bond(nodeC5, nodeC6, BondType.Single, Orientation.None));
                Bonds.Add(new Bond(nodeC6, nodeC1, BondType.Double, Orientation.None));
            }

            void Alkane(Node carbon, int length)
            {
                List<Node> carbonChain = new List<Node>();
                for (int i = 0; i < length; i++)
                {
                    carbonChain.Add(new Node(Elements.Carbon));
                }
                Bonds.Add(new Bond(carbon, carbonChain[0], BondType.Single, Orientation.Vertical));
                for (int i = 0; i < length - 1; i++)
                {
                    Bonds.Add(new Bond(carbonChain[i], carbonChain[i + 1], BondType.Single, Orientation.Vertical));
                }
                Nodes.AddRange(carbonChain);
            }

            void Ether(Node carbon, int length)
            {
                Node nodeO = new Node(Elements.Oxygen);
                List<Node> carbonChain = new List<Node>();
                for (int i = 0; i < length; i++)
                {
                    carbonChain.Add(new Node(Elements.Carbon));
                }
                Bonds.Add(new Bond(nodeO, carbonChain[0], BondType.Single, Orientation.Vertical));
                Bonds.Add(new Bond(carbon, nodeO, BondType.Single, Orientation.Vertical));
                for (int i = 0; i < length - 1; i++)
                {
                    Bonds.Add(new Bond(carbonChain[i], carbonChain[i + 1], BondType.Single, Orientation.Vertical));
                }
                Nodes.AddRange(carbonChain);
            }

            public Instructions GetModifiers(TokenizedExpression exp)
            {
                List<List<Token>> FunctionalGpTokenSet = new List<List<Token>>();
                Token ParentChainToken = exp.ParentChainTokens[0];
                List<List<Token>> TailTokenSet = new List<List<Token>>();

                FunctionalGpTokenSet.Add(new List<Token>());
                for (int i = 0; i < exp.FunctionalGpTokens.Count; i++)
                {
                    Token t = exp.FunctionalGpTokens[i];
                    FunctionalGps fgp;
                    if (t.Type is FunctionalGps)
                    {
                        fgp = (FunctionalGps)t.Type;
                        FunctionalGpTokenSet.Last().Add(t);
                        if (i != exp.FunctionalGpTokens.Count - 1)
                        {
                            if (exp.FunctionalGpTokens[i + 1].Type is FunctionalGps)
                            {
                                if (!CanModify((FunctionalGps)exp.FunctionalGpTokens[i + 1].Type))
                                {
                                    FunctionalGpTokenSet.Add(new List<Token>());
                                }
                            }
                            else if (!(exp.FunctionalGpTokens[i + 1].Type is ChemPrefixes))
                            {
                                FunctionalGpTokenSet.Add(new List<Token>());
                            }
                        }
                    }
                    else if (FunctionalGpTokenSet.Last().Count != 0 || !(t.Type is Operators) || !(((Operators)t.Type) == Operators.hyphen))
                    {
                        FunctionalGpTokenSet.Last().Add(t);
                    }
                }

                TailTokenSet.Add(new List<Token>());
                for (int i = 0; i < exp.TailTokens.Count; i++)
                {
                    Token t = exp.TailTokens[i];
                    if (t.Type is Bonds || t.Type is Suffixes)
                    {
                        TailTokenSet.Last().Add(t);
                        if (i < exp.TailTokens.Count - 1)
                            TailTokenSet.Add(new List<Token>());
                    }
                    else if (TailTokenSet.Last().Count != 0 || !(t.Type is Operators) || !(((Operators)t.Type) == Operators.hyphen))
                    {
                        TailTokenSet.Last().Add(t);
                    }
                }



                return null;
            }

            public Molecule GetRawMolecule(TokenizedExpression exp)
            {
                //Make parent chain
                List<Node> parentChain = new List<Node>();
                List<Bond> parentChainBonds = new List<Bond>();
                for (int i = 0; i < (int)exp.ParentChainTokens[0].Type; i++)
                {
                    Node tmp = new Node(Elements.Carbon);
                    Nodes.Add(tmp);
                    parentChain.Add(tmp);
                }
                for (int i = 0; i < parentChain.Count - 1; i++)
                {
                    Bond tmp = new Bond(parentChain[i], parentChain[i + 1], BondType.Single, Orientation.Horizontal);
                    parentChainBonds.Add(tmp);
                    Bonds.Add(tmp);
                }

                //make bonds
                int mode = 0;
                List<int> numbers = new List<int>();
                int engPreNum = 0;
                bool ignoreHyphen = true;
                for (int i = 0; i < exp.TailTokens.Count; i++)
                {
                    Token t = exp.TailTokens[i];
                    if (mode == 0)
                    {
                        Operators op;
                        Suffixes suffix;
                        EngPrefixes engPre;
                        if (t.Type is Operators)
                        {
                            op = (Operators)t.Type;
                            if (op == Operators.number)
                            {
                                numbers.Add(t.data);
                            }
                            else if (op == Operators.comma)
                            {

                            }
                            else if (op == Operators.hyphen && !ignoreHyphen)
                            {
                                mode = 1;
                            }
                        }
                        else if (t.Type is EngPrefixes)
                        {
                            engPre = (EngPrefixes)t.Type;
                            engPreNum = (int)engPre;
                        }
                        else if (t.Type is Suffixes)
                        {
                            suffix = (Suffixes)t.Type;
                            if (suffix == Suffixes.oic_acid)
                            {
                                OicAcid(parentChain[0]);
                                if (engPreNum == 2)
                                    OicAcid(parentChain.Last());
                            }
                            else if (suffix == Suffixes.carboxylic_acid)
                            {
                                CarboxylicAcid(parentChain[0]);
                                if (engPreNum == 2)
                                    CarboxylicAcid(parentChain.Last());
                            }
                            else if (suffix == Suffixes.al)
                            {
                                Aldehyde(parentChain[0]);
                                if (engPreNum == 2)
                                    Aldehyde(parentChain.Last());
                            }
                            else if (suffix == Suffixes.ol)
                            {
                                Alcohol(parentChain[0]);
                                if (engPreNum == 2)
                                    Alcohol(parentChain.Last());
                            }
                            else if (suffix == Suffixes.one)
                            {
                                Ketone(parentChain[0]);
                                if (engPreNum == 2)
                                    Ketone(parentChain.Last());
                            }
                            else if (suffix == Suffixes.amine)
                            {
                                Amine(parentChain[0]);
                                if (engPreNum == 2)
                                    Amine(parentChain.Last());
                            }
                            numbers.Clear();
                            engPreNum = 0;
                            mode = 0;
                        }
                        else
                        {
                            //TODO: implicit bond location
                            ignoreHyphen = true;
                        }
                    }
                    else if (mode == 1)
                    {
                        EngPrefixes engPrefix;
                        Bonds btype;
                        Suffixes suffix;
                        if (t.Type is Operators && !ignoreHyphen)
                        {
                            throw new FormatException("Wrong token format at token #" + i);
                        }
                        else if (t.Type is EngPrefixes)
                        {
                            engPrefix = (EngPrefixes)t.Type;
                            //TODO: check prefix validity
                        }
                        else if (t.Type is Bonds)
                        {
                            btype = (Bonds)t.Type;
                            foreach (int k in numbers)
                            {
                                parentChainBonds[k - 1].Type = (BondType)(int)btype;
                            }
                            numbers.Clear();
                            engPreNum = 0;
                            mode = 0;
                            ignoreHyphen = true;
                        }
                        else if (t.Type is Suffixes)
                        {
                            suffix = (Suffixes)t.Type;
                            if (suffix == Suffixes.ol)
                            {
                                foreach (int k in numbers)
                                {
                                    Alcohol(parentChain[k - 1]);
                                }
                            }
                            else if (suffix == Suffixes.carboxylic_acid)
                            {
                                foreach (int k in numbers)
                                {
                                    CarboxylicAcid(parentChain[k - 1]);
                                }
                            }
                            else if (suffix == Suffixes.one)
                            {
                                foreach (int k in numbers)
                                {
                                    Ketone(parentChain[k - 1]);
                                }
                            }
                            else if (suffix == Suffixes.amine)
                            {
                                foreach (int k in numbers)
                                {
                                    Amine(parentChain[k - 1]);
                                }
                            }
                            numbers.Clear();
                            engPreNum = 0;
                            mode = 0;
                            ignoreHyphen = true;
                        }
                    }
                    if (ignoreHyphen && (t.Type is Operators && (Operators)t.Type == Operators.hyphen))
                    {
                        ignoreHyphen = false;
                    }
                }

                //make functional groups
                mode = 0;
                numbers = new List<int>();
                engPreNum = 0;
                ignoreHyphen = false;
                for (int i = 0; i < exp.FunctionalGpTokens.Count; i++)
                {
                    Token t = exp.FunctionalGpTokens[i];
                    if (mode == 0)
                    {
                        Operators op;
                        EngPrefixes engPre;
                        FunctionalGps fGp;
                        if (t.Type is Operators)
                        {
                            op = (Operators)t.Type;
                            if (op == Operators.number)
                            {
                                numbers.Add(t.data);
                            }
                            else if (op == Operators.comma)
                            {

                            }
                            else if (op == Operators.hyphen && !ignoreHyphen)
                            {
                                mode = 1;
                            }
                        }
                        else if (t.Type is EngPrefixes)
                        {
                            engPre = (EngPrefixes)t.Type;
                            engPreNum = (int)engPre;
                        }
                        else if (t.Type is FunctionalGps)
                        {
                            fGp = (FunctionalGps)t.Type;
                            if (fGp == FunctionalGps.cyclo)
                            {
                                if (parentChain.First() != parentChain.Last() && !GetOther(parentChain.First()).Select(b => b.GetOther(parentChain.First())).Contains(parentChain.Last()))
                                    Bonds.Add(new Bond(parentChain.First(), parentChain.Last(), BondType.Single, Orientation.None));
                            }
                            //TODO: implicit functional group location
                            ignoreHyphen = true;
                        }
                    }
                    else if (mode == 1)
                    {
                        EngPrefixes engPrefix;
                        FunctionalGps ftype;
                        ChemPrefixes chemPrefix;
                        if (t.Type is Operators && !ignoreHyphen)
                        {
                            throw new FormatException("Wrong token format at token #" + i);
                        }
                        else if (t.Type is EngPrefixes)
                        {
                            engPrefix = (EngPrefixes)t.Type;
                            //TODO: check prefix validity
                        }
                        else if (t.Type is ChemPrefixes)
                        {
                            chemPrefix = (ChemPrefixes)t.Type;
                            if (i < exp.FunctionalGpTokens.Count - 1)
                            {
                                ftype = (FunctionalGps)exp.FunctionalGpTokens[i + 1].Type;
                                if (ftype == FunctionalGps.yl)
                                {
                                    foreach (int k in numbers)
                                    {
                                        Alkane(parentChain[k - 1], (int)chemPrefix);
                                    }
                                }
                                else if (ftype == FunctionalGps.oxy)
                                {
                                    foreach (int k in numbers)
                                    {
                                        Ether(parentChain[k - 1], (int)chemPrefix);
                                    }
                                }
                            }
                            numbers.Clear();
                            engPreNum = 0;
                            mode = 0;
                            ignoreHyphen = true;
                        }
                        else if (t.Type is FunctionalGps)
                        {
                            ftype = (FunctionalGps)t.Type;
                            if (ftype == FunctionalGps.bromo)
                            {
                                foreach (int k in numbers)
                                {
                                    Bromine(parentChain[k - 1]);
                                }
                            }
                            if (ftype == FunctionalGps.oxo)
                            {
                                foreach (int k in numbers)
                                {
                                    Ketone(parentChain[k - 1]);
                                }
                            }
                            else if (ftype == FunctionalGps.phenyl)
                            {
                                foreach (int k in numbers)
                                {
                                    Phenyl(parentChain[k - 1]);
                                }
                            }
                            else if (ftype == FunctionalGps.chloro)
                            {
                                foreach (int k in numbers)
                                {
                                    Chlorine(parentChain[k - 1]);
                                }
                            }
                            else if (ftype == FunctionalGps.fluoro)
                            {
                                foreach (int k in numbers)
                                {
                                    Fluorine(parentChain[k - 1]);
                                }
                            }
                            else if (ftype == FunctionalGps.hydroxy)
                            {
                                foreach (int k in numbers)
                                {
                                    Alcohol(parentChain[k - 1]);
                                }
                            }
                            numbers.Clear();
                            engPreNum = 0;
                            mode = 0;
                            ignoreHyphen = true;
                        }
                    }
                    if (ignoreHyphen && (t.Type is Operators && (Operators)t.Type == Operators.hyphen))
                    {
                        ignoreHyphen = false;
                    }
                }
                return this;
            }

            //Add hydrogen to all atoms that is not saturated
            public Molecule FixMolecule()
            {
                for (int i = 0; i < Nodes.Count; i++)
                {
                    Node n = Nodes[i];
                    int remain = GetBondCount(n.Type) - GetOther(n).Select(x => x.Type).Cast<int>().Sum();
                    for (int j = 0; j < remain; j++)
                    {
                        Node h = new Node(Elements.Hydrogen);
                        Bond b = new Bond(n, h, BondType.Single, Orientation.None);
                        Bonds.Add(b);
                        Nodes.Add(h);
                    }
                }
                return this;
            }

            public void FromName(string name)
            {
                StringExpression exp = Lexer(name);
                TokenizedExpression texp = Tokenize(exp);
                GetRawMolecule(texp);
                FixMolecule();
                GetModifiers(texp);
            }

            public List<Bond> GetOther(Node thisNode)
            {
                List<Bond> ret = new List<Bond>();
                foreach (Bond b in Bonds)
                {
                    Node n = b.GetOther(thisNode);
                    if (n != null) ret.Add(b);
                }
                return ret;
            }
        }



        public enum Orientation
        {
            Horizontal,
            Vertical,
            None
        }

        public class Bond
        {
            public BondType Type;
            public Node Node1;
            public Node Node2;
            public Orientation Orientation = Orientation.None;

            public Bond(Node thisNode, Node thatNode, BondType t, Orientation o)
            {
                Node1 = thisNode;
                Node2 = thatNode;
                Type = t;
                Orientation = o;
            }

            public Node GetOther(Node thisNode)
            {
                if (thisNode == Node1) return Node2;
                else if (thisNode == Node2) return Node1;
                else return null;
            }
        }

        public class Node
        {
            public Elements Type;

            public Node(Elements t)
            {
                Type = t;
            }
        }

        public static int GetBondCount(Elements type)
        {
            switch (type)
            {
                case Elements.Carbon:
                    return 4;
                case Elements.Bromine:
                case Elements.Chlorine:
                case Elements.Hydrogen:
                    return 1;
                case Elements.Nitrogen:
                    return 3;
                case Elements.Oxygen:
                    return 2;
                default:
                    return 0;
            }
        }

        public static bool CanModify(FunctionalGps t)
        {
            switch (t)
            {
                case FunctionalGps.oxy:
                case FunctionalGps.yl:
                case FunctionalGps.phenyl:
                    return true;
                default:
                    return false;
            }
        }
    }

    public enum BondType
    {
        Single = 1,
        Double = 2,
        Triple = 3
    }

    public enum Elements
    {
        [Description("C")]
        Carbon,
        [Description("H")]
        Hydrogen,
        [Description("O")]
        Oxygen,
        [Description("Cl")]
        Chlorine,
        [Description("Br")]
        Bromine,
        [Description("F")]
        Fluorine,
        [Description("N")]
        Nitrogen
    }
}
