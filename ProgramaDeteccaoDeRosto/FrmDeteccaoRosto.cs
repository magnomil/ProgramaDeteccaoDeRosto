using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProgramaDeteccaoDeRosto
{
    public partial class FrmDeteccaoRosto : Form
    {
        // criando um construtor para pegar o arquivo de treinamento
        static readonly CascadeClassifier cascadeClassifier = new CascadeClassifier("haarcascade_frontalface_alt_tree.xml");
        //pegando um vídeo de capture
        VideoCapture capture;

        public FrmDeteccaoRosto()
        {
            InitializeComponent();
        }

        private void abriImagemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Multiselect = false, Filter = "JPEG|*.jpg"})
            {
                if(ofd.ShowDialog()== DialogResult.OK)
                {
                   // picImagem.Image = Image.FromFile(ofd.FileName);
                    //Bitmap bitmap = new Bitmap(Image.FromFile(ofd.FileName));
                    //convertendo a imagem para uma rede de bites
                    ProcessarImagem(new Bitmap(Image.FromFile(ofd.FileName)));
                    

                }
            }
        }

        private void iniciarWebCamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // fazendo uma verificação na web cam se ela está reiniciada conforme o evento.
            {
                if(capture != null)
                {
                    MessageBox.Show("A web cam está iniciada!");
                        return;
                }
            }

            //iniciando um class para pegar a imagem, agora para colocar como parâmetro precisa do index da webcam. Como tenho uma webcam vai ser 0 dentro como index.
            capture = new VideoCapture(0);
            // o ImageGradded ele executa quando acha um frame da web cam. 
            capture.ImageGrabbed += Capture_ImageGrabbed;
            capture.Start();
        }

        //Esse evendo é chamado cada vez que um evento é capturado da webcam
        private void Capture_ImageGrabbed(object sender, EventArgs e)
        {
            try
            {
                Mat m = new Mat();
                capture.Retrieve(m);
                ProcessarImagem(m.Bitmap); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pararWebCamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(capture == null)
            {
                MessageBox.Show("A webCam está desligada");
                return;
            }
            capture.ImageGrabbed -= Capture_ImageGrabbed;
            capture.Stop();
            capture.Dispose();
            capture = null;
            picImagem.Image = null;
        }
            
        private void ProcessarImagem (Bitmap bitmap)
        {
            Image<Bgr, byte> grayImage = new Image<Bgr, byte>(bitmap);
            //vetor de retangulos e esse resultado de preenchimento de retangulo pelo arraycive. O detecteMultiescale é o xml de treinamento. Saber imagem processada e fator de escala.
            Rectangle[] rectangles = cascadeClassifier.DetectMultiScale(grayImage, 1.2, 1);
            //por meio dos retangulos encontrar retangulos para achar os arquivos de treinamentos.
            foreach (var retangle in rectangles)
            {
                //o using é um objeto criado para receber os desenhos dos rentângulos
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    using (Pen pen = new Pen(Color.Blue, 4))
                    {
                        graphics.DrawRectangle(pen, retangle);
                    }
                }
                {

                }
            }
            picImagem.Image = bitmap;


        }
    
    
    }

}

