using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace Solar
{
    public partial class SolarTrackers
    {
        Queue<string> Qx = new Queue<string>();
        Queue<double> Qy = new Queue<double>();
        List<string> Qx1 = new List<string>();
        List<double> Qy1 = new List<double>();
        List<string> Qx2 = new List<string>();
        List<double> Qy2 = new List<double>();
        private int a = 0;

        protected void InitChart()
        {
            ChartArea chartArea = NowChart.ChartAreas[0];// 设置显示范围
            chartArea.AxisY.Minimum = -26d;
            chartArea.AxisY.Maximum = 50d;

            if (toolBoxTable.Text == "温湿度信息")
            {
                ChartArea chartArea1 = chartLine.ChartAreas[0];// 设置显示范围
                chartLine.Series[0].Name = "环温";
                chartArea1.AxisY.Minimum = -26d;
                chartArea1.AxisY.Maximum = 50d;
            }

            if (toolBoxTable.Text == "辐射日累计信息")
            {
                ChartArea chartArea2 = chartLine.ChartAreas[1];
                chartArea2.AxisY.Minimum = 0d;
            }
        }

        protected void Draw()
        {
            this.NowChart.Series[0].Points.Clear();
            Qx.Enqueue(labTime.Text.Substring(11, 5));
            Qy.Enqueue(Convert.ToDouble(labTem.Text));
            int i;
            for (i = a; i < Qy.Count; i++) 
            {
                NowChart.Series[0].Points.AddXY(Qx.ElementAt(i), Qy.ElementAt(i));               
            }
            if (i > 6)
                a++;
        }

        protected void DrawLine()
        {
            chartLine.Series[0].Enabled = true;
            chartLine.Series[0].Name = "环温";
            this.chartLine.Series[0].Points.Clear();

            for (int r = 0; r < dataGridView1.Rows.Count; r++)
            {
                Qx1.Add(dataGridView1.Rows[r].Cells[0].Value.ToString());
                Qy1.Add(Convert.ToDouble(dataGridView1.Rows[r].Cells[1].Value));
            }

            for (int i = 0; i < Qy1.Count; i++)
            {
                chartLine.Series[0].Points.AddXY(Qx1.ElementAt(i), Qy1.ElementAt(i));
            }

            Qx1.Clear();
            Qy1.Clear();
        }
    }
}


