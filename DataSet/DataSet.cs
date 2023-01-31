namespace MachineLearningLib;

using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System;

public class DataSet<T1, T2> : IEnumerable<(T1[], T2)>
{
    public T1[][] X { get; private set; }
    public T2[] Y { get; private set; }
    public int Length => end - start;
    public int DataLength => X[0].Length;
    private int start;
    private int end;

    private DataSet() { }
    public DataSet(T1[][] X, T2[] Y)
    {
        this.X = X;
        this.Y = Y;
        this.start = 0;
        this.end = this.X.Length;
    }
    
    public static DataSet<T1, T2> Load(string path, char separator, string targetLabel)
    {
        var ds = new DataSet<T1, T2>();
        var data = DataSet<T1, T2>.open(path);
        
        ds.start = 0;
        ds.end = data.Count() - 1;
        ds.X = new T1[ds.Length - 1][];
        ds.Y = new T2[ds.Length - 1];

        int index = 0,
            labelIndex = data
            .First()
            .Split(separator)
            .Select((item, index) => (item, index))
            .First(i => i.item == targetLabel)
            .index;
            
        foreach (var line in data.Skip(1))
        {
            string[] lineData = line.Split(separator);
            var x = new float[lineData.Length - 1];
            int flag = 0;

            for (int i = 0; i < lineData.Length; i++)
            {
                int num = int.Parse(lineData[i]);

                if (i == labelIndex)
                {
                    ds.Y[index] = num;
                    flag++;
                }
                else
                    x[i - flag] = num;
            }

            ds.X[index] = x;
            index++;
        }

        return ds;
    }
    private static IEnumerable<string> open(string file)
    {
        var stream = new StreamReader(file);

        while (!stream.EndOfStream)
            yield return stream.ReadLine();

        stream.Close();
    }

    public (DataSet, DataSet) Split(float pct)
    {
        DataSet ds1 = new DataSet(this.X, this.Y);
        DataSet ds2 = new DataSet(this.X, this.Y);
        
        int div = (int)(pct * this.DataLength);
        ds1.end = div;
        ds2.start = div;

        return (ds1, ds2);
    }

    public DataSet RandSplit(float pct)
    {
        Random rand = new Random();
        DataSet dataset = new DataSet(X, Y);
        int div = (int)(pct * this.DataLength);

        var start = rand.Next(0, this.DataLength - div);
        var end = rand.Next(start + div, this.DataLength);

        dataset.start = start;
        dataset.end = end;

        return dataset;
    }
    
    public IEnumerator<(T1[], T2)> GetEnumerator()
    {
        for (int i = start; i < end; i++)
            yield return (X[i], Y[i]);
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}