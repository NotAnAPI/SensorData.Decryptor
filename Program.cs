using System.Text;
using System.Text.RegularExpressions;
using CommandLine;

namespace NotAnAPI.SensorData.Decryptor;

public class Decryptor
{
  public static async Task<int> Main(string[] args)
  {
    return await Parser.Default.ParseArguments<CommandLineOptions>(args)
        .MapResult(async opts => await Task.Run(() =>
            {
              try
              {
                string result = Decrypt(opts);
                Console.WriteLine(result);

                if (Console.ForegroundColor == ConsoleColor.DarkRed)
                {
                  Console.ForegroundColor = ConsoleColor.DarkYellow;
                  Console.WriteLine(Environment.NewLine + Environment.NewLine + "WARNING: It looks like the provided keys are wrong and failed to decrypt correctly.");
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "Press any key to exit");
                Console.ReadKey();
                return 0;
              }
              catch (Exception ex)
              {
                Console.WriteLine();
                Console.WriteLine("Error! Message:" + Environment.NewLine + ex.Message);
                Console.WriteLine(Environment.NewLine + Environment.NewLine + "Press any key to exit");
                Console.ReadKey();
                return -3; // Unhandled error
              }
            }),
            _ => Task.FromResult(-1)); // Invalid arguments
  }

  public static string Decrypt(in CommandLineOptions opts)
  {
    if (!File.Exists("sensor.txt"))
    {
      throw new Exception("sensor.txt File doesn't exists in the application root." + Environment.NewLine +
                          AppDomain.CurrentDomain.BaseDirectory);
    }

    string sensorFileContent = File.ReadAllText("sensor.txt", Encoding.UTF8);
    string[] splitFileContent = sensorFileContent.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    if (splitFileContent.Length > 1)
    {
      splitFileContent[1] = splitFileContent[1].Trim().Replace("bm_sz=", "");
    }

    int[] keys = string.IsNullOrEmpty(opts.BmSz)
        ? splitFileContent.Length > 2
            ?
            new[] { Convert.ToInt32(splitFileContent[1].Trim()), Convert.ToInt32(splitFileContent[2].Trim()) }
            : splitFileContent.Length > 1
                ? ExtractEncryptionKeys($"bm_sz={splitFileContent[1]}; ")
                : opts.FirstEncryptionKey > 1 && opts.SecondEncryptionKey > 1 ? new[] { opts.FirstEncryptionKey, opts.SecondEncryptionKey } : new[] { 8888888, 7777777 }
        : ExtractEncryptionKeys($"bm_sz={(opts.BmSz.Replace("bm_sz=", ""))}; ");

    if (keys[0] == 8888888 && keys[1] == 7777777)
    {
      Console.ForegroundColor = ConsoleColor.DarkYellow;
      Console.WriteLine("* No encryption keys provided, using default keys *");
    }

    Console.WriteLine($"First key = {keys[0]} | Second key = {keys[1]}" + Environment.NewLine);

    string decrypted = splitFileContent[0].Trim();
    if (decrypted.StartsWith("{\"sensor_data\":\"", StringComparison.OrdinalIgnoreCase) && decrypted.EndsWith("\"}"))
    {
      decrypted = decrypted.Substring("{\"sensor_data\":\"".Length, decrypted.Length - 2 - "{\"sensor_data\":\"".Length);
    }

    var match = Regex.Match(decrypted, @"^([\d]+\;[\d]+\;[\d]+\;[\d]+\,[\d]+\,[\d]+\,[\d]+\,[\d]+;)", RegexOptions.Singleline);

    if (match.Success)
    {
      decrypted = decrypted[match.Groups[1].Value.Length..];
    }


    var decryptd = Wq(in decrypted, keys[0]);

    var fixedOrder = Gq(in decryptd, keys[1]);

    if (fixedOrder.IndexOf("PiZtE", StringComparison.OrdinalIgnoreCase) > -1 && fixedOrder.IndexOf("7a74", StringComparison.OrdinalIgnoreCase) > -1)
    {
      Console.ForegroundColor = ConsoleColor.DarkGreen;
      fixedOrder = MakeItEvenBetter(in fixedOrder);
    }
    else
    {
      Console.ForegroundColor = ConsoleColor.DarkRed;
    }

    if (match.Success)
    {
      fixedOrder = match.Groups[1].Value + fixedOrder;
    }

    return fixedOrder;
  }

  private static string MakeItEvenBetter(in string sensor)
  {
    var result = sensor;
    // 1
    var regex = new Regex(@",-112,(.{2,25})http[s]*:\/", RegexOptions.Singleline | RegexOptions.IgnoreCase);
    var match = regex.Match(sensor);
    if (match.Success)
    {
      result = sensor.Replace(match.Groups[1].Value, "");
    }

    return result;
  }

  private static string Wq(in string xq, int nq)
  {
    var b6 = "";
    string m6 = "";
    JsArray j6 = new(16);

    if (string.IsNullOrEmpty(m6))
    {
      for (var i = 0; i < 127; ++i)
      {
        if (i is < 32 or 39 or 34 or 92)
        {
          j6[i] = -1;
        }
        else
        {
          j6[i] = m6.Length;
          m6 += Convert.ToChar(i).ToString();
        }
      }
    }

    for (var i = 0; i < xq.Length; ++i)
    {
      var a6 = Convert.ToInt32(Gf(Lc(nq, 8), 65535));
      nq *= 65793;
      nq &= To32Bit(4294967295);
      nq += 4282663;
      nq &= 8388607;
      var yq = xq[i].ToString();
      for (var j = 1; j <= m6.Length; j++)
      {
        var st = (a6 % m6.Length + j);
        if (st % m6.Length != j6[CharCodeAt(xq, i)]) continue;
        var ls = st - a6 % m6.Length;
        yq = m6[ls >= m6.Length ? ls - m6.Length : ls].ToString();
      }

      b6 += yq;
    }

    return b6;
  }

  private static string Gq(in string lq, int zq)
  {
    var oq = lq.Split(",");
    int[] m1 = new int[oq.Length];
    int[] m2 = new int[oq.Length];
    for (var vq = 0; vq < oq.Length; vq++)
    {
      var iq = Convert.ToInt32(Bc(Gf(Lc(zq, 8), 65535), oq.Length));
      zq *= 65793;
      zq &= To32Bit(4294967295);
      zq += 4282663;
      zq &= 8388607;
      var mq = Convert.ToInt32(Bc(Gf(Lc(zq, 8), 65535), oq.Length));
      zq *= 65793;
      zq &= To32Bit(4294967295);
      zq += 4282663;
      zq &= 8388607;
      m1[oq.Length - 1 - vq] = iq;
      m2[oq.Length - 1 - vq] = mq;
    }

    for (var i = 0; i < m1.Length; i++)
    {
      (oq[m2[i]], oq[m1[i]]) = (oq[m1[i]], oq[m2[i]]);
    }

    return Join(oq, ",");
  }

  private static decimal Bc(int b8, int m8)
  {
    return b8 % m8;
  }

  private static int Gf(int sf, int gF)
  {
    return sf & gF;
  }

  private static int Lc(int mc, int zc)
  {
    return mc >> zc;
  }

  private static int[] ExtractEncryptionKeys(in string cookie)
  {
    var encryptionKeys = new[] { 8888888, 7777777 };
    var pq = GetCookie("bm_sz", cookie);
    if (pq is bool or false) return encryptionKeys;
    try
    {
      var split = Uri.UnescapeDataString((string)pq).Split("~");
      if (split.Length >= 4)
      {
        var fq = Convert.ToInt32(split[2], 10);
        var qq = Convert.ToInt32(split[3], 10);
        encryptionKeys = new[] { fq, qq };
      }
    }
    catch
    {
      // ignored
    }

    return encryptionKeys;
  }

  private static dynamic GetCookie(in string ki, in string cookie)
  {
    if (string.IsNullOrEmpty(cookie)) return false;
    var ti = ki + "=";
    var hi = cookie.Split("; ");
    foreach (var xI in hi)
    {
      if (0 != xI.IndexOf(ti, StringComparison.CurrentCultureIgnoreCase)) continue;
      var ui = xI[ti.Length..];
      if (-1 != ui.IndexOf("~", StringComparison.InvariantCulture) || -1 !=
          Uri.UnescapeDataString(ui).IndexOf("~", StringComparison.InvariantCulture))
      {
        return ui;
      }
    }

    return false;
  }

  private static string Join<T>(T[] list, string delimiter)
  {
    string toReturn = "";
    for (int i = 0; i < list.Length; i++)
    {
      toReturn += list[i]?.ToString();
      if (i < list.Length - 1)
        toReturn += delimiter;
    }

    return toReturn;
  }

  private static int CharCodeAt(string input, int index)
  {
    return index >= input.Length ? 0 : input[index];
  }

  private static int To32Bit(uint input)
  {
    return BitConverter.ToInt32(BitConverter.GetBytes(input), 0);
  }
}

public class JsArray
{
  private readonly List<dynamic> _arr;

  public JsArray(int size)
  {
    _arr = new List<dynamic>(size);
    this[size - 1] = null!;
  }

  public dynamic this[int index]
  {
    get => (_arr.Count > index ? _arr[index] : null)!;
    set
    {
      if (_arr.Count <= index)
      {
        var totalToAdd = index - _arr.Count;
        for (int iAdd = 0; iAdd <= totalToAdd; iAdd++)
        {
          _arr.Add(null!);
        }
      }

      _arr[index] = value;
    }
  }
}