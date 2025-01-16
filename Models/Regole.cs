using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoCasinoGava.Models
{
    class Regole
    {
        #region =01=== costanti e statici =====================


        #endregion
        #region=02=== membri e propietà ===========propful====
        private string _nome;

        public string Nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        private string _come;

        public string Come
        {
            get { return _come; }
            set { _come = value; }
        }

        private int _multiplicatore;

        public int Multiplicatore
        {
            get { return _multiplicatore; }
            set { _multiplicatore = value; }
        }

        private string _image;

        public string Image
        {
            get { return _image; }
            set { _image = value; }
        }
        #endregion
        #region=03=== costruttori ============================

        #endregion
        #region=04=== metodi privati e aiuti =================

        #endregion
        #region=05=== metodi public ==========================
        public Regole(string nome, string come, int multiplicatore, string image)
        {
            Nome = nome;
            Come = come;
            Multiplicatore = multiplicatore;
            Image = image;

        }
        #endregion



    }
}
