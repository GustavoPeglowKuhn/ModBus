using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ModBus {
    class Motor {
        Timer t;
        int star_Time, current_Time;

        Motor() {
            t.Enabled = false;
            t.Interval = 500;   //mudar valor depois
			t.Elapsed+=delegate {
				t_Tick();
			};
        }

        public void ligar(int time){
            star_Time = time;
            t.Enabled = true;
        }

        private void t_Tick() {
            //ler tempo do motor nokit
            // current_Time = "read_Time" 
            if (current_Time >= star_Time) {

                // trocar para triangulo
                t.Enabled = false;
            }
        }
    }
}
