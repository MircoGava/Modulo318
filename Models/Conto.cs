using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgettoCasinoGava.Models
{
    internal class Conto
    {
        #region =01=== costanti e statici =====================

        #endregion
        #region=02=== membri e propietà ===========propful====
        private int _banca;

		public int Banca
		{
			get { return _banca; }
			set { _banca = value; }
		}

		private int _puntata;

		public int Puntata
		{
			get { return _puntata; }
			set { _puntata = value; }
		}
        #endregion
        #region=03=== costruttori ============================
        
        #endregion
        #region=04=== metodi privati e aiuti =================

        #endregion
        #region=05=== metodi public ==========================
        public Conto(int banca, int puntata)
        {
            Banca = banca;
            Puntata = puntata;
        }
        #endregion

    }
}
