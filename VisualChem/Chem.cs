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
    class Chem
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

        static Dictionary<string,object> dictAllFunctionalGps =
        dictOperators.ToDictionary(p => p.Key, p => (object)p.Value)
        .Union(dictVowels.ToDictionary(p => p.Key, p => (object)p.Value))
        .Union(dictEngPrefixes.ToDictionary(p => p.Key, p => (object)p.Value))
        .Union(dictChemPrefixes.ToDictionary(p => p.Key, p => (object)p.Value))
        .Union(dictFunctionalGps.ToDictionary(p => p.Key, p => (object)p.Value)).ToDictionary(s => s.Key,s => s.Value);

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
            di,
            tri,
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
            dodec = 12
            tridec = 13,
            tetradec = 14,
            pentadec = 15,
            hexadec = 16,
            heptadec = 17,
            octadec = 18,
            nonadec = 19,
            icos = 20,
        }
        public enum FunctionalGps
        {
            yl, oxy, hydroxy, carboxyl, bromo, chloro
        }
        public enum Bonds
        {
            an, en, yn
        }
        public enum Suffixes
        {
            [Description("oic acid")]
            oic_acid,
            ol,
            oate,
            al,
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

        public class Molecule
        {
            public List<Node> Nodes = new List<Node>();
            public List<Bond> Bonds = new List<Bond>();

            public Molecule()
            {

            }

            List<string> FindStringTokens(string name, List<string> refTokens)
            {
                if (name.Length == 0)
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
                Match m = Regex.Match(name, "^([a-z\\-,0-9()]*)(?:((?:^|(?<=[a-z)]))(?:" + Enum.GetValues(typeof(ChemPrefixes)).Cast<ChemPrefixes>().Select((op) => op.ToDString()).Aggregate((a, b) => a + "|" + b) + ")(?!yl|oxy))([a-z\\-,0-9 ]*))$");
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

            public Molecule GetMolecule(TokenizedExpression exp)
            {
                List<Node> parentChain = new List<Node>();

            }

            public void InitFromName(string name)
            {

            }

            public List<Node> GetOther(Node thisNode)
            {
                List<Node> ret = new List<Node>();
                foreach (Bond b in Bonds)
                {
                    Node n = b.GetOther(thisNode);
                    if (n != null) ret.Add(n);
                }
                return ret;
            }
        }

        public enum BondType
        {
            Single,
            Double,
            Triple
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
            public Orientation Orienation = Orientation.None;

            public Node GetOther(Node thisNode)
            {
                if (thisNode == Node1) return Node2;
                else if (thisNode == Node2) return Node1;
                else return null;
            }
        }

        public enum NodeType
        {
            Carbon,
            Hydrogen,
            Oxygen,
            Chlorine,
            Bromine,
            Nitrogen
        }

        public class Node
        {
            public NodeType Type;

        }
    }
}
