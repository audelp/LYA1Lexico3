using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel.DataAnnotations.Schema;

namespace LYA1_Lexico3
{
    public class Lexico : Token, IDisposable
    {
        const int F = -1;
        const int E = -2;
        private StreamReader archivo;
        private StreamWriter log;


        /*
        int[,] TRAND =  
        {
        //  0,1,2,3 ,4,5 ,6 ,7,8 ,9
        // WS,L,D,. ,E,+ ,- ,=,; ,La
           {0,1,2,16,1,16,16,8,10,11,12,13,16}, // 0
           {F,1,1,F,1,F ,F ,F ,F ,F,F,F,F }, // 1
           {F,F,2,3,5,F ,F ,F ,F ,F,F,F,F }, // 2
           {E,E,4,E,E,E ,E ,E ,E ,E,E,E,E }, // 3
           {F,F,4,F,5,F ,F ,F ,F ,F,F,F,F }, // 4
           {E,E,7,E,E,6 ,6 ,E ,E ,E,E,E,E }, // 5
           {E,E,7,E,E,E ,E ,E ,E ,E,E,E,E }, // 6
           {F,F,7,F,F,F ,F ,F ,F ,F,F,F,F }, // 7
           {F,F,F,F,F,F ,F ,9 ,F ,F,F,F,F }, // 8
           {F,F,F,F,F,F ,F ,F ,F ,F,F,F,F }, // 9
           {F,F,F,F,F,F ,F ,F ,F ,F,F,F,F }, // 10
           {F,F,F,F,F,F ,F ,F ,F ,F,F,F,F }, // 11
        };
*/

        int[,] TRAND =  
        {
    //   0  1   2   3   4   5   6   7   8   9   10  11  12  13  14  15  16  17  18  19  20  21  22  23
    //   WS	L	D	.	E	+	-	=	;	&	|	!	>	<	*	/	%	?	"	{	}	La	EOF EOL
        {0 ,1 ,	2 ,	27,	1 ,	19,	20,	8 ,	10,	11,	12,	13,	16,	17,	22,	28,	22,	24,	25,	32,	33,	27,	F, 0},//0
        {F ,1 ,	1 ,	F ,	1 ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//1
        {F ,F ,	2 ,	3 ,	5 ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//2
        {E ,E ,	4 ,	E ,	E ,	E ,	E ,	E ,	E ,	E ,	E ,	E ,	E,	E,	E,	E,	E,	E,	E,	E,	E,	E,	F, 0},//3
        {F ,F ,	4 ,	F ,	5 ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//4
        {E ,E ,	7 ,	E ,	E ,	6 ,	6 ,	E ,	E ,	E ,	E ,	E ,	E,	E,	E,	E,	E,	E,	E,	E,	E,	E,	F, 0},//5
        {E ,E ,	7 ,	E ,	E ,	E ,	E ,	E ,	E ,	E ,	E ,	E ,	E,	E,	E,	E,	E,	E,	E,	E,	E,	E,	F, 0},//6
        {F ,F ,	7 ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//7
        {F ,F ,	F ,	F ,	F ,	F ,	F ,	9 ,	F ,	F ,	F ,	F ,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//8
        {F ,F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//9
        {F ,F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//10
        {F ,F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	14,	F ,	F ,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//11
        {F ,F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	14,	F ,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//12
        {F ,F ,	F ,	F ,	F ,	F ,	F ,	15,	F ,	F ,	F ,	F ,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//13
        {F ,F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//14
        {F ,F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//15
        {F ,F ,	F ,	F ,	F ,	F ,	F ,	18,	F ,	F ,	F ,	F ,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//16
        {F ,F ,	F ,	F ,	F ,	F ,	F ,	18,	F ,	F ,	F ,	F ,	18,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//17
        {F ,F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//18
        {F ,F ,	F ,	F ,	F ,	21,	F ,	21,	F ,	F ,	F ,	F ,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//19
        {F ,F ,	F ,	F ,	F ,	F ,	21,	21,	F ,	F ,	F ,	F ,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//20
        {F ,F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//21
        {F ,F ,	F ,	F ,	F ,	F ,	F ,	23,	F ,	F ,	F ,	F ,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//22
        {F ,F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//23
        {F ,F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//24
        {25,25,	25,	25,	25,	25,	25,	25,	25,	25,	25,	25,	25,	25,	25,	25,	25,	25,	26,	25,	25,	25,	F, 0},//25
        {F ,F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//26
        {F ,F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//27
        {F ,F ,	F ,	F ,	F ,	F ,	F ,	23,	F ,	F ,	F ,	F ,	F,	F,	30,	29,	F,	F,	F,	F,	F,	F,	F, 0},//28
        {29,29,	29,	29,	29,	29,	29,	29,	29,	29,	29,	29,	29,	29,	29,	29,	29,	29,	29,	29,	29,	29,	F, F},//29
        {30,30,	30,	30,	30,	30,	30,	30,	30,	30,	30,	30,	30,	30,	31,	30,	30,	30,	30,	30,	30,	30,	E, 0},//30
        {30,30,	30,	30,	30,	30,	30,	30,	30,	30,	30,	30, 30, 30,	31,	32,	30,	30,	30, 30, 30,	30,	E, 0},//31
        {F ,F ,	F ,	F ,	F , F ,	F ,	F ,	F ,	F , F ,	F ,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//32
        {F ,F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//33
        {F ,F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F ,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F,	F, 0},//34 

        };

        public Lexico()
        {
            archivo = new StreamReader("prueba.cpp");
            log = new StreamWriter("prueba.log");
            log.AutoFlush = true;
        }
        public Lexico(string nombre)
        {
            archivo = new StreamReader(nombre);
            log = new StreamWriter("prueba.log");
            log.AutoFlush = true;
        }
        public void Dispose()
        {
            archivo.Close();
            log.Close();
        }
        private int columna(char c)
        {   
            if(c=='\n')
                return 23;
            else if(archivo.EndOfStream)
                return 22;
            else if (char.IsWhiteSpace(c))
                return 0;
            else if (char.ToLower(c) == 'e')
                return 4;
            else if (char.IsLetter(c))
                return 1;
            else if (char.IsAsciiDigit(c))
                return 2;
            else if (c=='.')
                return 3;
            else if (c=='+')
                return 5;
            else if (c=='-')
                return 6;
            else if (c=='=')
                return 7;
            else if (c==';')
                return 8;
            else if(c=='&')
                return 9;
            else if(c=='|')
                return 10;
            else if(c=='!')
                return 11;
            else if(c=='>')
                return 12;
            else if(c=='<')
                return 13;
            else if(c=='*')
                return 14;
            else if(c=='/')
                return 15;
            else if(c=='%')
                return 16;
            else if(c=='?')
                return 17;
            else if(c=='\"')
                return 18;
            else if(c=='{')
                return 20;
            else if(c=='}')
                return 21;
            else
                return 22;
        }
        private void clasificar(int estado)
        {
            switch (estado)
            {
                case 1: setClasificacion(Tipos.Identificador); break;
                case 2: setClasificacion(Tipos.Numero); break;
                case 8: setClasificacion(Tipos.asignacion); break;
                case 9: setClasificacion(Tipos.opRelacional); break;
                case 10: setClasificacion(Tipos.finSentencia); break;
                case 11: setClasificacion(Tipos.Caracter); break;
                case 12: setClasificacion(Tipos.Caracter); break;
                case 13: setClasificacion(Tipos.Caracter); break;
                case 14: setClasificacion(Tipos.opLogico); break;
                case 15: setClasificacion(Tipos.opRelacional); break;
                case 16: setClasificacion(Tipos.opRelacional); break;
                case 17: setClasificacion(Tipos.opRelacional); break;
                case 19: setClasificacion(Tipos.opTermino); break;
                case 20: setClasificacion(Tipos.opTermino); break;
                case 21: setClasificacion(Tipos.incTermino); break;
                case 22: setClasificacion(Tipos.opFactor); break;
                case 23: setClasificacion(Tipos.incFactor); break;
                case 24: setClasificacion(Tipos.opTernario); break;
                case 25: setClasificacion(Tipos.cadena); break;
                case 26: setClasificacion(Tipos.cadena); break;
                case 27: setClasificacion(Tipos.Caracter); break;
                case 28: setClasificacion(Tipos.Caracter); break;
                case 29: setClasificacion(Tipos.comentario); break;
                case 30: setClasificacion(Tipos.comentario); break;
                case 31: setClasificacion(Tipos.comentario); break;
                case 32: setClasificacion(Tipos.comentario); break;
                case 33: setClasificacion(Tipos.llaveInicio); break;
                case 34: setClasificacion(Tipos.llaveFin); break;


            }
        }
        public void nextToken()
        {
            char c;
            string buffer = "";

            int estado = 0;

            while (estado >= 0)
            {
                c = (char)archivo.Peek();

                estado = TRAND[estado,columna(c)];
                clasificar(estado);
                
                if (estado >= 0)
                {
                    if (estado > 0)
                    {
                        buffer += c;    
                    }
                    else
                        buffer = "";
                    archivo.Read();
                }
            }
            if (estado == E)
            {
                throw new Error("Lexico: Se espera un digito",log);
            }
            setContenido(buffer);
            log.WriteLine(getContenido() + " = " + getClasificacion());
        }
        public bool FinArchivo()
        {
            return archivo.EndOfStream;
        }
    }
}