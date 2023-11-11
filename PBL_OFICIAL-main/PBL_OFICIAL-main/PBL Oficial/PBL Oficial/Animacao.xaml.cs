using OxyPlot.Annotations;
using OxyPlot.Series;
using OxyPlot;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Windows.Media;

namespace WpfApp1
{

    public partial class MainWindow : Window
    {
        private PlotModel model;
        private LineSeries series;
        private EllipseAnnotation ball;
        private EllipseAnnotation projectile;
        private double ballPositionX;
        private double ballPositionY;
        private double projectilePositionX;
        private double projectilePositionY;

        public MainWindow()
        {
            InitializeComponent();

            // Configurar o modelo do gráfico
            model = new PlotModel { Title = "Gráfico X e Y" };
            series = new LineSeries();
            model.Series.Add(series);

            // Configurar a posição inicial da bola
            ballPositionX = 20;
            ballPositionY = 40;
            ball = new EllipseAnnotation
            {
                X = ballPositionX,
                Y = ballPositionY,
                Width = 2,
                Height = 4,
                Stroke = OxyColors.Black,
                Fill = OxyColors.Red
            };
            model.Annotations.Add(ball);

            // Configurar a posição inicial do projetil
            projectilePositionX = 0;
            projectilePositionY = 0;
            projectile = new EllipseAnnotation
            {
                X = projectilePositionX,
                Y = projectilePositionY,
                Width = 2,
                Height = 4,
                Stroke = OxyColors.Black,
                Fill = OxyColors.Blue
            };
            model.Annotations.Add(projectile);

            // Associar o modelo ao plotView(Local onde gráfico sera gerado)
            plotView.Model = model;

            // Aguardar um pequeno atraso antes de iniciar ambas as animações

            AnimateProjectile();
            AnimateBall();

        }

        private async void AnimateProjectile()
        {
            double initialX = projectilePositionX;
            double initialY = projectilePositionY;
            double targetX = 20; // Ajuste conforme necessário
            double targetY = 20; // Ajuste conforme necessário
            double fallDurationMs = 1000;

            DateTime startTime = DateTime.Now;

            while (projectilePositionX < targetX || projectilePositionY < targetY)
            {
                double progress = (DateTime.Now - startTime).TotalMilliseconds / fallDurationMs;
                projectilePositionX = initialX + (targetX - initialX) * progress;
                projectilePositionY = initialY + (targetY - initialY) * progress;

                // Verificar se o projetil atingiu as coordenadas desejadas
                if (projectilePositionX >= targetX && projectilePositionY >= targetY)
                {
                    projectilePositionX = targetX;
                    projectilePositionY = targetY;
                    break; // Interrompe a animação do projetil quando atinge as coordenadas desejadas
                }

                //Atualiza o trajeto e joga assimila gráfico
                projectile.X = projectilePositionX;
                projectile.Y = projectilePositionY;
                plotView.InvalidatePlot();
                await Task.Delay(16);
            }
        }

        private async void AnimateBall()
        {
            double initialY = 40; // Posição inicial da bola
            double finalY = 20;   // Posição final da bola
            int animationDurationMs = 1000;
            DateTime startTime = DateTime.Now;

            while (ballPositionY > finalY)
            {
                double progress = (DateTime.Now - startTime).TotalMilliseconds / animationDurationMs;
                ballPositionY = initialY - (initialY - finalY) * progress;

                // Verificar se a bola atingiu ou ultrapassou a posição final desejada
                if (ballPositionY <= finalY)
                {
                    ballPositionY = finalY;
                }

                ball.Y = ballPositionY;
                plotView.InvalidatePlot();
                await Task.Delay(16);
            }
        }
    }
}

