public class Day_21
{
    //Multiple answers work bc of int division, and this was the lowest functional number
    public const ulong PART_TWO_ANS = 3887609741189;
    public void Main()
    {
        string[] lines = File.ReadAllLines("Day_21_Input.txt");

        Day_21_02(lines);
    }

    void Day_21_01(string[] input)
    {
        Dictionary<string, Phrase> monkeys = new();

        for(int i = 0; i < input.Length; i++)
        {
            string[] splitInput = input[i].Split(' ');

            string name = splitInput[0].Substring(0, 4);

            if(splitInput.Length == 2)
            {
                Phrase newConstantPhrase = new Phrase(Convert.ToUInt64(splitInput[1]));
                monkeys.Add(name, newConstantPhrase);
            }
            else
            {
                Operation newOp;
                switch (splitInput[2])
                {
                    case "+":
                        newOp = Operation.ADD;
                        break;
                    case "-":
                        newOp = Operation.SUB;
                        break;
                    case "*":
                        newOp = Operation.MUL;
                        break;
                    case "/":
                        newOp = Operation.DIV;
                        break;
                    default:
                        throw new Exception("FAILED INPUT READ");
                }
                Phrase newComplexPhrase = new Phrase(splitInput[1], splitInput[3], newOp);
                monkeys.Add(name, newComplexPhrase);
            }
        }

        ulong result = monkeys["root"].Eval(monkeys);
        Console.WriteLine("Result: " + result);
    }

    void Day_21_02(string[] input)
    {
        //NUM I NEED TO MATCH: 32310041242752
        //humn should be around: 3888000000000 (3.888x10^12)

        Dictionary<string, Phrase> monkeys = new();

        for (int i = 0; i < input.Length; i++)
        {
            string[] splitInput = input[i].Split(' ');

            string name = splitInput[0].Substring(0, 4);

            if (splitInput.Length == 2)
            {
                Phrase newConstantPhrase = new Phrase(Convert.ToUInt64(splitInput[1]));
                monkeys.Add(name, newConstantPhrase);
            }
            else
            {
                Operation newOp;
                switch (splitInput[2])
                {
                    case "+":
                        newOp = Operation.ADD;
                        break;
                    case "-":
                        newOp = Operation.SUB;
                        break;
                    case "*":
                        newOp = Operation.MUL;
                        break;
                    case "/":
                        newOp = Operation.DIV;
                        break;
                    default:
                        throw new Exception("FAILED INPUT READ");
                }
                Phrase newComplexPhrase = new Phrase(splitInput[1], splitInput[3], newOp);
                monkeys.Add(name, newComplexPhrase);
            }
        }

        Console.WriteLine(PART_TWO_ANS);
        ulong lhs = monkeys[monkeys["root"].lhs].Eval2(monkeys);
        Console.WriteLine(lhs);
        ulong result = monkeys[monkeys["root"].rhs].Eval2(monkeys);
        Console.WriteLine("Result: " + result);
        Console.WriteLine($"{(lhs == result ? "TRUE" : "FALSE")}");
    }

    public enum Operation
    {
        ADD,
        SUB,
        MUL,
        DIV
    }

    public class Phrase
    {
        public bool IsConstant;

        public ulong value = 0;

        public Operation operation = Operation.ADD;
        public string lhs = "";
        public string rhs = "";

        public Phrase(ulong constVal)
        {
            IsConstant = true;
            value = constVal;
        }

        public Phrase(string lhs, string rhs, Operation o)
        {
            IsConstant = false;

            this.lhs = lhs;
            this.rhs = rhs;
            this.operation = o;
        }


        public ulong Eval(Dictionary<string, Phrase> monkeys)
        {
            if (IsConstant)
            {
                return value;
            }


            switch (operation)
            {
                case Operation.ADD:

                    return monkeys[lhs].Eval(monkeys) + monkeys[rhs].Eval(monkeys);

                case Operation.SUB:

                    return monkeys[lhs].Eval(monkeys) - monkeys[rhs].Eval(monkeys);

                case Operation.MUL:

                    return monkeys[lhs].Eval(monkeys) * monkeys[rhs].Eval(monkeys);

                case Operation.DIV:

                    return monkeys[lhs].Eval(monkeys) / monkeys[rhs].Eval(monkeys);

                default:
                    throw new Exception("INVALID OPERATION");
            }
        }

        public ulong Eval2(Dictionary<string, Phrase> monkeys)
        {
            if (IsConstant)
            {
                return value;
            }

            ulong left;
            ulong right;

            if (lhs == "humn")
            {
                left = PART_TWO_ANS;
            }
            else
            {
                left = monkeys[lhs].Eval2(monkeys);
            }

            switch (operation)
            {
                case Operation.ADD:

                    return left + monkeys[rhs].Eval2(monkeys);

                case Operation.SUB:

                    return left - monkeys[rhs].Eval2(monkeys);

                case Operation.MUL:

                    return left * monkeys[rhs].Eval2(monkeys);

                case Operation.DIV:

                    return left / monkeys[rhs].Eval2(monkeys);

                default:
                    throw new Exception("INVALID OPERATION");
            }
        }

        public string StringEval(Dictionary<string, Phrase> monkeys)
        {
            if (IsConstant)
            {
                return Convert.ToString(value);
            }

            string left;
            string right;

            if(lhs == "humn")
            {
                left = "humn";
            }
            else
            {
                left = monkeys[lhs].StringEval(monkeys);
                if (!left.Contains("humn"))
                {
                    left = Convert.ToString(monkeys[lhs].Eval(monkeys));
                }
            }

            if (rhs == "humn")
            {
                right = "humn";
            }
            else
            {
                right = monkeys[rhs].StringEval(monkeys);
                if (!right.Contains("humn"))
                {
                    right = Convert.ToString(monkeys[rhs].Eval(monkeys));
                }
            }


            switch (operation)
            {
                case Operation.ADD:

                    return $"({left} + {right})";

                case Operation.SUB:

                    return $"({left} - {right})";

                case Operation.MUL:

                    return $"({left} * {right})";

                case Operation.DIV:

                    return $"({left} / {right})";

                default:
                    throw new Exception("INVALID OPERATION");
            }
        }
    }
}