using Interpreter.Helpers;
using Interpreter.Interface;
using Interpreter.Service;

namespace Interpreter.Test
{
    static class InterpreterTest
    {
        //=======================================================================================================
        // RUN TESTS
        //=======================================================================================================

        public static void RunTestSuiteCompleta(Dictionary<string, IInternalFunction> funcs)
        {
            Console.WriteLine("\n=== Test: Suite completa Example01 (torture test) ===");
            var interpreter = InterpreterRunner.HydrateScript(ScriptSuiteCompleta(), funcs);
            InterpreterRunner.RunExecution(interpreter, new Dictionary<string, object?>());
        }

        public static void RunTestMutacionBasica(Dictionary<string, IInternalFunction> funcs)
        {
            Console.WriteLine("\n=== Test: Mutación básica ===");
            var interpreter = InterpreterRunner.HydrateScript(ScriptMutacionBasica(), funcs); // LA HIDRATACION, SOLO CUANDO SE ASIGNA EL SCRIPT
            InterpreterRunner.RunExecution(interpreter, ExternalDataCliente());
        }

        public static void RunTestControlFlow(Dictionary<string, IInternalFunction> funcs)
        {
            Console.WriteLine("\n=== Test: Control de flujo ===");
            var interpreter = InterpreterRunner.HydrateScript(ScriptControlFlow(), funcs);
            InterpreterRunner.RunExecution(interpreter, new Dictionary<string, object?>());
        }

        public static void RunTestColecciones(Dictionary<string, IInternalFunction> funcs)
        {
            Console.WriteLine("\n=== Test: Listas y objetos ===");
            var interpreter = InterpreterRunner.HydrateScript(ScriptCollections(), funcs);
            InterpreterRunner.RunExecution(interpreter, new Dictionary<string, object?>());
        }

        public static void RunTestTopCategorias(Dictionary<string, IInternalFunction> funcs)
        {
            Console.WriteLine("\n=== Test: Función externa TopCategorias (lista de objetos) ===");
            var interpreter = InterpreterRunner.HydrateScript(ScriptTopCategorias(), funcs);
            InterpreterRunner.RunExecution(interpreter, ExternalDataPreferencia());
        }

        public static void RunTestJson(Dictionary<string, IInternalFunction> funcs)
        {
            Console.WriteLine("\n=== Test: JsonParse / JsonStringify ===");
            var interpreter = InterpreterRunner.HydrateScript(ScriptJson(), funcs);
            InterpreterRunner.RunExecution(interpreter, new Dictionary<string, object?>());
        }

        public static void RunTestJsonExterno(Dictionary<string, IInternalFunction> funcs)
        {
            Console.WriteLine("\n=== Test: Función externa AplicarDescuento (lista de objetos) ===");
            var interpreter = InterpreterRunner.HydrateScript(ScriptJsonExterno(), funcs);
            var result = InterpreterRunner.RunExecution(interpreter, ExternalDataDescuentoClienteA());

            Console.WriteLine($"\nJSON RESULTANTE: {JsonHelper.Stringify(result)}\n");
        }

        public static void RunTestJsonExternoSinHidratacion(Dictionary<string, IInternalFunction> funcs)
        {
            Console.WriteLine("\n=== Test: Función externa AplicarDescuento (SIN HIDRATACION) ===");
            var interpreter = InterpreterRunner.HydrateScript(ScriptJsonExterno(), funcs);

            var result = InterpreterRunner.RunExecution(interpreter, ExternalDataDescuentoClienteA());
            Console.WriteLine($"\nJSON RESULTANTE: {JsonHelper.Stringify(result)}\n");

            var result1 = InterpreterRunner.RunExecution(interpreter, ExternalDataDescuentoClienteB());
            Console.WriteLine($"\nJSON RESULTANTE: {JsonHelper.Stringify(result1)}\n");

            var result2 = InterpreterRunner.RunExecution(interpreter, ExternalDataDescuentoClienteA());
            Console.WriteLine($"\nJSON RESULTANTE: {JsonHelper.Stringify(result2)}\n");
        }

        //=======================================================================================================
        // SCRIPTS
        //=======================================================================================================

        public static string ScriptSuiteCompleta()
        {
            return TextMinifyHelper.Minify(@"
                Mostrar(""=== SUITE COMPLETA ==="");
                int numero = 42;
                string texto = ""Hola Mundo"";
                bool esVerdadero = true;
                bool esFalso = false;
                
                Mostrar(numero);
                Mostrar(texto);
                Mostrar(esVerdadero);
                Mostrar(esFalso);

                Mostrar(""=== TEST 2: ARITMETICA ==="");
                int suma = 10 + 5;
                int resta = 20 - 8;
                int multiplicacion = 6 * 7;
                int division = 100 / 4;
                int modulo = 17 % 5;
                int compleja = 10 + 5 * 2 - 8 / 4;
                
                Mostrar(suma);
                Mostrar(resta);
                Mostrar(multiplicacion);
                Mostrar(division);
                Mostrar(modulo);
                Mostrar(compleja);

                Mostrar(""=== TEST 3: COMPARACIONES ==="");
                int a = 10;
                int b = 20;
                
                IF(a < b)
                {
                    Mostrar(""10 es menor que 20"");
                }
                
                IF(a > b)
                {
                    Mostrar(""NO DEBERIA VERSE"");
                }
                ELSE
                {
                    Mostrar(""10 NO es mayor que 20"");
                }
                
                IF(a == 10)
                {
                    Mostrar(""a es igual a 10"");
                }
                
                IF(a != b)
                {
                    Mostrar(""a es diferente de b"");
                }
                
                IF(a <= 10)
                {
                    Mostrar(""a es menor o igual a 10"");
                }
                
                IF(b >= 20)
                {
                    Mostrar(""b es mayor o igual a 20"");
                }

                Mostrar(""=== TEST 4: LOGICA ==="");
                bool verdad = true;
                bool mentira = false;
                
                IF(verdad AND verdad)
                {
                    Mostrar(""true AND true = true"");
                }
                
                IF(verdad AND mentira)
                {
                    Mostrar(""NO DEBERIA VERSE"");
                }
                ELSE
                {
                    Mostrar(""true AND false = false"");
                }
                
                IF(verdad OR mentira)
                {
                    Mostrar(""true OR false = true"");
                }
                
                IF(mentira OR mentira)
                {
                    Mostrar(""NO DEBERIA VERSE"");
                }
                ELSE
                {
                    Mostrar(""false OR false = false"");
                }
                
                IF(NOT mentira)
                {
                    Mostrar(""NOT false = true"");
                }

                Mostrar(""=== TEST 5: IF ANIDADO ==="");
                int puntuacion = 85;
                
                IF(puntuacion >= 90)
                {
                    Mostrar(""Calificacion: A"");
                }
                ELSEIF(puntuacion >= 80)
                {
                    Mostrar(""Calificacion: B"");
                }
                ELSEIF(puntuacion >= 70)
                {
                    Mostrar(""Calificacion: C"");
                }
                ELSEIF(puntuacion >= 60)
                {
                    Mostrar(""Calificacion: D"");
                }
                ELSE
                {
                    Mostrar(""Calificacion: F"");
                }

                Mostrar(""=== TEST 6: SWITCH ==="");
                int dia = 3;
                
                SWITCH(dia)
                {
                    CASE(1):
                        Mostrar(""Lunes"");
                    CASE(2):
                        Mostrar(""Martes"");
                    CASE(3):
                        Mostrar(""Miercoles"");
                    CASE(4):
                        Mostrar(""Jueves"");
                    CASE(5):
                        Mostrar(""Viernes"");
                    CASE(6):
                        Mostrar(""Sabado"");
                    CASE(7):
                        Mostrar(""Domingo"");
                    DEFAULT:
                        Mostrar(""Dia invalido"");
                }
                
                string color = ""rojo"";
                SWITCH(color)
                {
                    CASE(""rojo""):
                        Mostrar(""El color es rojo"");
                    CASE(""verde""):
                        Mostrar(""El color es verde"");
                    CASE(""azul""):
                        Mostrar(""El color es azul"");
                    DEFAULT:
                        Mostrar(""Color desconocido"");
                }

                Mostrar(""=== TEST 7: WHILE ==="");
                int contador = 0;
                WHILE(contador < 5)
                {
                    Mostrar(contador);
                    contador = contador + 1;
                }
                
                int factorial = 1;
                int n = 5;
                int i = 1;
                WHILE(i <= n)
                {
                    factorial = factorial * i;
                    i = i + 1;
                }
                Mostrar(""Factorial de 5:"");
                Mostrar(factorial);

                Mostrar(""=== TEST 8: FOR ==="");
                FOR(int x = 0; x < 5; x = x + 1)
                {
                    Mostrar(x);
                }
                
                Mostrar(""Tabla del 5:"");
                FOR(int j = 1; j <= 10; j = j + 1)
                {
                    int resultadoTabla = 5 * j;
                    Mostrar(resultadoTabla);
                }

                Mostrar(""=== TEST 9: SCOPES ==="");
                int global = 100;
                Mostrar(global);
                
                FOR(int local = 0; local < 3; local = local + 1)
                {
                    int dentroFor = 999;
                    Mostrar(local);
                    Mostrar(global);
                }

                Mostrar(""=== TEST 10: ANIDAMIENTO ==="");
                int nivel1 = 1;
                
                IF(nivel1 == 1)
                {
                    int nivel2 = 2;
                    Mostrar(nivel2);
                    
                    FOR(int nivel3 = 0; nivel3 < 2; nivel3 = nivel3 + 1)
                    {
                        Mostrar(nivel3);
                        
                        WHILE(nivel3 == 1)
                        {
                            Mostrar(""Dentro del WHILE anidado"");
                            nivel3 = nivel3 + 1;
                        }
                    }
                }

                Mostrar(""=== TEST 11: STRINGS ==="");
                string nombre = ""Juan"";
                string apellido = ""Perez"";
                string nombreCompleto = nombre + "" "" + apellido;
                Mostrar(nombreCompleto);
                
                string mensaje = ""Hola "" + nombre + "", tienes "" + 30 + "" años"";
                Mostrar(mensaje);

                Mostrar(""=== TEST 12: EXPRESIONES COMPLEJAS ==="");
                int x1 = 10;
                int y1 = 20;
                int z1 = 30;
                
                IF((x1 < y1 AND y1 < z1) OR x1 == 10)
                {
                    Mostrar(""Expresion compleja evaluada correctamente"");
                }
                
                int calculo = (10 + 20) * 2 - 15 / 3;
                Mostrar(calculo);

                Mostrar(""=== TEST 13: NEGACION ==="");
                int positivo = 50;
                int negativo = -positivo;
                Mostrar(negativo);
                
                int resultadoNeg = -10 + 20;
                Mostrar(resultadoNeg);

                Mostrar(""=== TEST 14: REASIGNACION ==="");
                int variable = 10;
                Mostrar(variable);
                
                variable = 20;
                Mostrar(variable);
                
                variable = variable + 5;
                Mostrar(variable);
                
                variable = variable * 2;
                Mostrar(variable);

                Mostrar(""=== TEST 15: BUCLES CONDICIONALES ==="");
                int suma2 = 0;
                FOR(int k = 1; k <= 10; k = k + 1)
                {
                    IF(k % 2 == 0)
                    {
                        suma2 = suma2 + k;
                    }
                }
                Mostrar(""Suma de numeros pares del 1 al 10:"");
                Mostrar(suma2);

                Mostrar(""=== TEST 16: SALIDA MULTIPLE ==="");
                Mostrar(""Linea 1"");
                Mostrar(""Linea 2"");
                Mostrar(""Linea 3"");
                Mostrar(123);
                Mostrar(true);
                Mostrar(false);

                Mostrar(""=== TEST 17: EDGE CASES ARITMETICOS ==="");
                int cero = 0;
                int uno = 1;
                int negativo1 = -1;
                
                Mostrar(""Multiplicacion por 0:"", 5 * cero);
                Mostrar(""Multiplicacion por 1:"", 5 * uno);
                Mostrar(""Multiplicacion por -1:"", 5 * negativo1);
                Mostrar(""Division por 1:"", 100 / uno);
                Mostrar(""Suma con negativos:"", 10 + negativo1);
                Mostrar(""Resta con negativos:"", 10 - negativo1);
                
                int expresionCompleja = -5 * 3 + 20 / 4 - 2;
                Mostrar(""Expresion con negativos:"", expresionCompleja);

                Mostrar(""=== TEST 18: COMPARACIONES EXTREMAS ==="");
                int minimo = -100;
                int maximo = 100;
                int medio = 0;
                
                IF(minimo < medio AND medio < maximo)
                {
                    Mostrar(""Rango correcto: -100 < 0 < 100"");
                }
                
                IF(minimo == minimo)
                {
                    Mostrar(""Igualdad consigo mismo"");
                }
                
                IF(NOT (minimo > maximo))
                {
                    Mostrar(""Negacion de comparacion falsa"");
                }

                Mostrar(""=== TEST 19: STRINGS ESPECIALES ==="");
                string vacio = """";
                string espacios = ""   "";
                string numerosStr = ""12345"";
                string especiales = ""!@#$%"";
                
                Mostrar(""Cadena vacia:"", vacio, ""<fin"");
                Mostrar(""Espacios:"", espacios, ""<fin"");
                Mostrar(""Numeros como string:"", numerosStr);
                Mostrar(""Caracteres especiales:"", especiales);
                
                string concatenada = vacio + ""hola"" + espacios + ""mundo"";
                Mostrar(""Concatenacion compleja:"", concatenada);

                Mostrar(""=== TEST 20: LOGICA BOOLEANA AVANZADA ==="");
                bool a1 = true;
                bool b1 = false;
                bool c1 = true;
                
                IF(a1 AND b1 OR c1)
                {
                    Mostrar(""(true AND false) OR true = true"");
                }
                
                IF(NOT (a1 AND b1))
                {
                    Mostrar(""NOT (true AND false) = true"");
                }
                
                IF((a1 OR b1) AND (c1 OR b1))
                {
                    Mostrar(""(true OR false) AND (true OR false) = true"");
                }

                Mostrar(""=== TEST 21: SWITCH EXHAUSTIVO ==="");
                
                FOR(int testDia = 0; testDia <= 8; testDia = testDia + 1)
                {
                    SWITCH(testDia)
                    {
                        CASE(1):
                            Mostrar(""Caso 1"");
                        CASE(2):
                            Mostrar(""Caso 2"");
                        CASE(3):
                            Mostrar(""Caso 3"");
                        CASE(4):
                            Mostrar(""Caso 4"");
                        CASE(5):
                            Mostrar(""Caso 5"");
                        DEFAULT:
                            Mostrar(""Default para:"", testDia);
                    }
                }

                Mostrar(""=== TEST 22: WHILE COMPLEJO ==="");
                int a2 = 0;
                int b2 = 10;
                
                WHILE(a2 < 5 AND b2 > 5)
                {
                    Mostrar(""a2:"", a2, ""b2:"", b2);
                    a2 = a2 + 1;
                    b2 = b2 - 1;
                }
                
                int contador2 = 0;
                WHILE(contador2 < 3 OR contador2 == 0)
                {
                    Mostrar(""Contador OR:"", contador2);
                    contador2 = contador2 + 1;
                    IF(contador2 > 5)
                    {
                        contador2 = 10;
                    }
                }

                Mostrar(""=== TEST 23: FOR DECREMENTAL ==="");
                FOR(int dec = 10; dec > 0; dec = dec - 1)
                {
                    Mostrar(""Cuenta regresiva:"", dec);
                }
                
                FOR(int dec2 = 20; dec2 >= 10; dec2 = dec2 - 2)
                {
                    Mostrar(""Decremento de 2:"", dec2);
                }

                Mostrar(""=== TEST 24: SCOPES PROFUNDOS ==="");
                int nivel0 = 0;
                
                IF(nivel0 == 0)
                {
                    int nivel1Var = 1;
                    
                    IF(nivel1Var == 1)
                    {
                        int nivel2Var = 2;
                        
                        IF(nivel2Var == 2)
                        {
                            int nivel3Var = 3;
                            
                            IF(nivel3Var == 3)
                            {
                                int nivel4Var = 4;
                                Mostrar(""Nivel 4 alcanzado:"", nivel4Var);
                                Mostrar(""Acceso a nivel 0:"", nivel0);
                            }
                        }
                    }
                }

                Mostrar(""=== TEST 25: MATEMATICAS COMPLEJAS ==="");
                int resultado1 = ((10 + 5) * 3 - 8) / 2 + 10;
                Mostrar(""Resultado 1:"", resultado1);
                
                int resultado2 = 100 - (50 / 2) + (20 * 3) - 15;
                Mostrar(""Resultado 2:"", resultado2);
                
                int resultado3 = (((2 + 3) * 4) - 5) / 3;
                Mostrar(""Resultado 3:"", resultado3);
                
                int a3 = 10;
                int b3 = 20;
                int c3 = 30;
                int resultado4 = (a3 + b3) * c3 / (b3 - a3);
                Mostrar(""Resultado 4:"", resultado4);

                Mostrar(""=== TEST 26: CONDICIONES ENCADENADAS ==="");
                int valor = 50;
                
                IF(valor > 0)
                {
                    Mostrar(""Mayor que 0"");
                    
                    IF(valor > 25)
                    {
                        Mostrar(""Mayor que 25"");
                        
                        IF(valor > 40)
                        {
                            Mostrar(""Mayor que 40"");
                            
                            IF(valor == 50)
                            {
                                Mostrar(""Es exactamente 50"");
                            }
                        }
                    }
                }

                Mostrar(""=== TEST 27: BUCLES MULTI-CONDICION ==="");
                int x2 = 0;
                int y2 = 10;
                
                WHILE(x2 < 5 AND y2 > 0)
                {
                    IF(x2 % 2 == 0)
                    {
                        Mostrar(""Par x2:"", x2, ""y2:"", y2);
                    }
                    ELSE
                    {
                        Mostrar(""Impar x2:"", x2, ""y2:"", y2);
                    }
                    x2 = x2 + 1;
                    y2 = y2 - 1;
                }

                Mostrar(""=== TEST DE LISTAS BASICAS ==="");
                list miLista = [];

                ListAdd(miLista, 10);
                ListAdd(miLista, ""Hola"");
                ListAdd(miLista, true);
                ListAdd(miLista, 42);

                int total = ListCount(miLista);
                Mostrar(""Total elementos:"", total);

                FOR(int idx = 0; idx < ListCount(miLista); idx = idx + 1)
                {
                    Mostrar(""Elemento"", idx, "":"", miLista[idx]);
                }

                Mostrar(""=== TEST 28: LISTAS CON OPERACIONES ==="");
                list numerosLista = [];
                
                FOR(int n1 = 1; n1 <= 5; n1 = n1 + 1)
                {
                    ListAdd(numerosLista, n1 * 10);
                }
                
                int sumaLista = 0;
                FOR(int n2 = 0; n2 < ListCount(numerosLista); n2 = n2 + 1)
                {
                    int valorActual = numerosLista[n2];
                    sumaLista = sumaLista + valorActual;
                    Mostrar(""Elemento:"", valorActual, ""- Suma acumulada:"", sumaLista);
                }

                Mostrar(""=== TEST 29: LISTAS EDGE CASES ==="");
                list listaVacia = [];
                Mostrar(""Lista vacia, count:"", ListCount(listaVacia));
                
                ListAdd(listaVacia, ""primer elemento"");
                Mostrar(""Despues de agregar, count:"", ListCount(listaVacia));
                
                ListClear(listaVacia);
                Mostrar(""Despues de limpiar, count:"", ListCount(listaVacia));
                
                ListAdd(listaVacia, 1);
                ListAdd(listaVacia, 2);
                ListAdd(listaVacia, 3);
                Mostrar(""Antes de remover, count:"", ListCount(listaVacia));
                
                ListRemoveAt(listaVacia, 1);
                Mostrar(""Despues de remover indice 1, count:"", ListCount(listaVacia));

                Mostrar(""=== TEST DE OBJETOS BASICOS ==="");

                object persona = { nombre: ""Juan"", edad: 30, activo: true };

                Mostrar(""Nombre:"", persona.nombre);
                Mostrar(""Edad:"", persona.edad);
                Mostrar(""Activo:"", persona.activo);

                Mostrar(""=== TEST 30: LISTA DE OBJETOS ==="");
                list personas = [];
                ListAdd(personas, { nombre: ""Maria"", edad: 25 });
                ListAdd(personas, { nombre: ""Pedro"", edad: 35 });
                ListAdd(personas, { nombre: ""Ana"", edad: 28 });

                FOR(int i = 0; i < ListCount(personas); i = i + 1)
                {
                    object p = personas[i];
                    Mostrar(""Persona:"", p.nombre, ""-"", p.edad);
                }

                Mostrar(""=== TEST 31: OBJETOS MIXTOS ==="");
                object producto = { 
                    id: 100, 
                    nombre: ""Laptop"", 
                    precio: 1500, 
                    disponible: true,
                    descripcion: ""Laptop de alta gama""
                };
                
                Mostrar(""ID:"", producto.id);
                Mostrar(""Nombre:"", producto.nombre);
                Mostrar(""Precio:"", producto.precio);
                Mostrar(""Disponible:"", producto.disponible);
                Mostrar(""Descripcion:"", producto.descripcion);

                Mostrar(""=== TEST 32: LISTA DE PRODUCTOS COMPLETA ==="");

                list productos = [];

                ListAdd(productos, { id: 1, nombre: ""Laptop"", precio: 1500, stock: 10 });
                ListAdd(productos, { id: 2, nombre: ""Mouse"", precio: 25, stock: 50 });
                ListAdd(productos, { id: 3, nombre: ""Teclado"", precio: 75, stock: 30 });
                ListAdd(productos, { id: 4, nombre: ""Monitor"", precio: 300, stock: 15 });
                ListAdd(productos, { id: 5, nombre: ""Webcam"", precio: 80, stock: 25 });

                FOR(int i = 0; i < ListCount(productos); i = i + 1)
                {
                    object prod = productos[i];
                    Mostrar(""ID:"", prod.id, ""- Nombre:"", prod.nombre, ""- Precio:"", prod.precio, ""- Stock:"", prod.stock);
                }

                int valorTotal = 0;
                FOR(int i = 0; i < ListCount(productos); i = i + 1)
                {
                    object prod = productos[i];
                    int valorProducto = prod.precio * prod.stock;
                    valorTotal = valorTotal + valorProducto;
                }
                Mostrar(""Valor total del inventario:"", valorTotal);

                Mostrar(""=== TEST 33: FILTRADO DE PRODUCTOS ==="");
                Mostrar(""Productos con stock bajo (menos de 20):"");
                
                FOR(int i = 0; i < ListCount(productos); i = i + 1)
                {
                    object prod = productos[i];
                    IF(prod.stock < 20)
                    {
                        Mostrar(""ALERTA -"", prod.nombre, ""- Stock:"", prod.stock);
                    }
                }
                
                Mostrar(""Productos caros (mas de 100):"");
                FOR(int i = 0; i < ListCount(productos); i = i + 1)
                {
                    object prod = productos[i];
                    IF(prod.precio > 100)
                    {
                        Mostrar(""PREMIUM -"", prod.nombre, ""- Precio:"", prod.precio);
                    }
                }

                Mostrar(""=== TEST 34: LISTA DE EMPLEADOS ==="");

                list empleados = [];

                ListAdd(empleados, { nombre: ""Juan Perez"", edad: 35, salario: 3000, activo: true });
                ListAdd(empleados, { nombre: ""Maria Lopez"", edad: 28, salario: 3500, activo: true });
                ListAdd(empleados, { nombre: ""Carlos Ruiz"", edad: 42, salario: 4000, activo: false });
                ListAdd(empleados, { nombre: ""Ana Torres"", edad: 31, salario: 3200, activo: true });
                ListAdd(empleados, { nombre: ""Luis Gomez"", edad: 45, salario: 4500, activo: false });

                Mostrar(""Empleados activos:"");
                int contadorActivos = 0;
                FOR(int i = 0; i < ListCount(empleados); i = i + 1)
                {
                    object emp = empleados[i];
                    IF(emp.activo == true)
                    {
                        Mostrar(""- "", emp.nombre, "" (Edad:"", emp.edad, "", Salario:"", emp.salario, "")"");
                        contadorActivos = contadorActivos + 1;
                    }
                }
                Mostrar(""Total empleados activos:"", contadorActivos);

                int sumaTotal = 0;
                FOR(int i = 0; i < ListCount(empleados); i = i + 1)
                {
                    object emp = empleados[i];
                    sumaTotal = sumaTotal + emp.salario;
                }
                int promedio = sumaTotal / ListCount(empleados);
                Mostrar(""Salario promedio:"", promedio);

                Mostrar(""=== TEST 35: EMPLEADOS POR RANGO DE EDAD ==="");
                
                int jovenes = 0;
                int adultos = 0;
                int seniors = 0;
                
                FOR(int i = 0; i < ListCount(empleados); i = i + 1)
                {
                    object emp = empleados[i];
                    
                    IF(emp.edad < 30)
                    {
                        jovenes = jovenes + 1;
                    }
                    ELSEIF(emp.edad >= 30 AND emp.edad < 40)
                    {
                        adultos = adultos + 1;
                    }
                    ELSE
                    {
                        seniors = seniors + 1;
                    }
                }
                
                Mostrar(""Jovenes (menos de 30):"", jovenes);
                Mostrar(""Adultos (30-39):"", adultos);
                Mostrar(""Seniors (40+):"", seniors);

                Mostrar(""=== TEST 36: LISTA DE TRANSACCIONES ==="");

                list transacciones = [];

                ListAdd(transacciones, { tipo: ""deposito"", monto: 1000, fecha: ""2025-01-15"" });
                ListAdd(transacciones, { tipo: ""retiro"", monto: 250, fecha: ""2025-01-16"" });
                ListAdd(transacciones, { tipo: ""deposito"", monto: 500, fecha: ""2025-01-17"" });
                ListAdd(transacciones, { tipo: ""retiro"", monto: 100, fecha: ""2025-01-18"" });
                ListAdd(transacciones, { tipo: ""deposito"", monto: 750, fecha: ""2025-01-19"" });
                ListAdd(transacciones, { tipo: ""retiro"", monto: 300, fecha: ""2025-01-20"" });

                int balance = 0;
                int totalDepositos = 0;
                int totalRetiros = 0;
                int cantidadDepositos = 0;
                int cantidadRetiros = 0;
                
                FOR(int i = 0; i < ListCount(transacciones); i = i + 1)
                {
                    object trans = transacciones[i];
    
                    IF(trans.tipo == ""deposito"")
                    {
                        balance = balance + trans.monto;
                        totalDepositos = totalDepositos + trans.monto;
                        cantidadDepositos = cantidadDepositos + 1;
                        Mostrar(""[+]"", trans.fecha, ""- Deposito:"", trans.monto, ""- Balance:"", balance);
                    }
                    ELSE
                    {
                        balance = balance - trans.monto;
                        totalRetiros = totalRetiros + trans.monto;
                        cantidadRetiros = cantidadRetiros + 1;
                        Mostrar(""[-]"", trans.fecha, ""- Retiro:"", trans.monto, ""- Balance:"", balance);
                    }
                }

                Mostrar(""=== RESUMEN ==="");
                Mostrar(""Balance final:"", balance);
                Mostrar(""Total depositado:"", totalDepositos);
                Mostrar(""Total retirado:"", totalRetiros);
                Mostrar(""Cantidad de depositos:"", cantidadDepositos);
                Mostrar(""Cantidad de retiros:"", cantidadRetiros);

                Mostrar(""=== TEST 37: OBJETOS ANIDADOS ==="");

                list pedidos = [];

                object cliente1 = { nombre: ""Juan"", telefono: ""123456"" };
                object producto1 = { nombre: ""Laptop"", precio: 1500 };

                ListAdd(pedidos, { 
                    id: 1, 
                    cliente: cliente1, 
                    producto: producto1, 
                    cantidad: 2 
                });

                object cliente2 = { nombre: ""Maria"", telefono: ""789012"" };
                object producto2 = { nombre: ""Mouse"", precio: 25 };

                ListAdd(pedidos, { 
                    id: 2, 
                    cliente: cliente2, 
                    producto: producto2, 
                    cantidad: 5 
                });

                FOR(int i = 0; i < ListCount(pedidos); i = i + 1)
                {
                    object pedido = pedidos[i];
                    object cliente = pedido.cliente;
                    object producto3 = pedido.producto;
                    
                    Mostrar(""Pedido ID:"", pedido.id);
                    Mostrar(""Cliente:"", cliente.nombre, ""- Tel:"", cliente.telefono);
                    Mostrar(""Producto:"", producto3.nombre, ""- Precio:"", producto3.precio);
                    Mostrar(""Cantidad:"", pedido.cantidad);
                    
                    int totalPedido = producto3.precio * pedido.cantidad;
                    Mostrar(""Total pedido:"", totalPedido);
                    Mostrar(""---"");
                }

                Mostrar(""=== TEST 38: BUSQUEDA EN LISTA ==="");

                list usuarios = [];
                ListAdd(usuarios, { id: 1, nombre: ""Juan"", email: ""juan@mail.com"" });
                ListAdd(usuarios, { id: 2, nombre: ""Maria"", email: ""maria@mail.com"" });
                ListAdd(usuarios, { id: 3, nombre: ""Pedro"", email: ""pedro@mail.com"" });
                ListAdd(usuarios, { id: 4, nombre: ""Ana"", email: ""ana@mail.com"" });

                int buscarId = 2;
                bool encontrado = false;

                FOR(int i = 0; i < ListCount(usuarios); i = i + 1)
                {
                    object user = usuarios[i];
                    IF(user.id == buscarId)
                    {
                        Mostrar(""Usuario encontrado:"", user.nombre, ""-"", user.email);
                        encontrado = true;
                    }
                }

                IF(encontrado == false)
                {
                    Mostrar(""Usuario no encontrado"");
                }
                
                Mostrar(""Buscar ID inexistente:"");
                buscarId = 999;
                encontrado = false;
                
                FOR(int i = 0; i < ListCount(usuarios); i = i + 1)
                {
                    object user = usuarios[i];
                    IF(user.id == buscarId)
                    {
                        encontrado = true;
                    }
                }
                
                IF(encontrado == false)
                {
                    Mostrar(""Usuario"", buscarId, ""no encontrado"");
                }

                Mostrar(""=== TEST 39: MODIFICACION DE PROPIEDADES ==="");
                object config = { modo: ""normal"", nivel: 1, activo: false };
                
                Mostrar(""Estado inicial:"");
                Mostrar(""Modo:"", config.modo);
                Mostrar(""Nivel:"", config.nivel);
                Mostrar(""Activo:"", config.activo);
                
                config.modo = ""avanzado"";
                config.nivel = 5;
                config.activo = true;
                
                Mostrar(""Estado modificado:"");
                Mostrar(""Modo:"", config.modo);
                Mostrar(""Nivel:"", config.nivel);
                Mostrar(""Activo:"", config.activo);

                Mostrar(""=== TEST 40: CONTADORES Y ESTADISTICAS ==="");
                
                list ventas = [];
                ListAdd(ventas, { producto: ""A"", cantidad: 10, precio: 100 });
                ListAdd(ventas, { producto: ""B"", cantidad: 5, precio: 200 });
                ListAdd(ventas, { producto: ""C"", cantidad: 15, precio: 50 });
                ListAdd(ventas, { producto: ""D"", cantidad: 8, precio: 150 });
                
                int totalVentas = 0;
                int cantidadTotal = 0;
                int ventaMayor = 0;
                string productoMayor = """";
                
                FOR(int i = 0; i < ListCount(ventas); i = i + 1)
                {
                    object venta = ventas[i];
                    int montoVenta = venta.cantidad * venta.precio;
                    totalVentas = totalVentas + montoVenta;
                    cantidadTotal = cantidadTotal + venta.cantidad;
                    
                    IF(montoVenta > ventaMayor)
                    {
                        ventaMayor = montoVenta;
                        productoMayor = venta.producto;
                    }
                    
                    Mostrar(""Producto:"", venta.producto, ""- Monto:"", montoVenta);
                }
                
                Mostrar(""=== ESTADISTICAS ==="");
                Mostrar(""Total ventas:"", totalVentas);
                Mostrar(""Cantidad total vendida:"", cantidadTotal);
                Mostrar(""Venta mayor:"", ventaMayor, ""- Producto:"", productoMayor);

                Mostrar(""=== TODAS LAS PRUEBAS COMPLETADAS ==="");
            ");
        }

        public static string ScriptMutacionBasica()
        {
            return TextMinifyHelper.Minify(@"
                object cliente = input.cliente;
                cliente.saldo = cliente.saldo + 100.01;
                object bonificacion = { tipo: ""bonificacion"", monto: 100 };
                ListAdd(input.transacciones, bonificacion);
                object output = input;
            ");
        }

        public static string ScriptControlFlow()
        {
            return TextMinifyHelper.Minify(@"
                Mostrar(""=== IF / ELSEIF / ELSE ==="");
                int score = 82;
                IF(score >= 90) { Mostrar(""A""); }
                ELSEIF(score >= 80) { Mostrar(""B""); }
                ELSE { Mostrar(""C o menos""); }
                Mostrar(""=== SWITCH ==="");
                int dia = 3;
                SWITCH(dia)
                {
                    CASE(1): Mostrar(""Lunes"");
                    CASE(2): Mostrar(""Martes"");
                    CASE(3): Mostrar(""Miercoles"");
                    DEFAULT: Mostrar(""Otro"");
                }
                Mostrar(""=== WHILE ==="");
                int i = 0;
                WHILE(i < 3)
                {
                    Mostrar(""i ="", i);
                    i = i + 1;
                }
                Mostrar(""=== FOR ==="");
                FOR(int j = 0; j < 3; j = j + 1)
                {
                    Mostrar(""j ="", j);
                }
                object output = { score: score, dia: dia };
            ");
        }

        public static string ScriptCollections()
        {
            return TextMinifyHelper.Minify(@"
                list numeros = [];
                ListAdd(numeros, 10);
                ListAdd(numeros, 20);
                ListAdd(numeros, 30);
                Mostrar(""Cantidad:"", ListCount(numeros));
                int primero = numeros[0];
                Mostrar(""Primero:"", primero);
                object persona = { nombre: ""Ana"", edad: 28 };
                Mostrar(""Nombre:"", persona.nombre, ""Edad:"", persona.edad);
                persona.edad = persona.edad + 1;
                object output = { numeros: numeros, persona: persona };
            ");
        }

        public static string ScriptJson()
        {
            return TextMinifyHelper.Minify(@"
                object obj = { id: 1, nombre: ""Widget"", precio: 99.5 };
                obj.precio = obj.precio + 10;
                string jsonSalida = JsonStringify(obj);
                Mostrar(""JSON modificado:"", jsonSalida);
                object output = obj;
            ");
        }

        public static string ScriptJsonExterno()
        {
            return TextMinifyHelper.Minify(@"
                list descuentos = [];

                object req1 = { monto: input.precio, factor: input.factor };
                object res1 = AplicarDescuento(req1);
                ListAdd(descuentos, res1);
                Mostrar(""Descuento 1:"", res1.resultado);

                IF(res1.monto > 1000)
                {
                    object req2 = { monto: input.precio2, factor: input.factor2 * 2 };
                    object res2 = AplicarDescuento(req2);
                    ListAdd(descuentos, res2);
                    Mostrar(""Descuento 2:"", res2.resultado);
                }
                ELSE
                {
                    Mostrar(""Se omite segundo descuento"");
                }

                object output = { descuentos: descuentos };
            ");
        }

        public static string ScriptTopCategorias()
        {
            return TextMinifyHelper.Minify(@"
                list categorias = TopCategorias({ preferencia: input.preferencia });
                Mostrar(""Total categorias:"", ListCount(categorias));
                object primera = categorias[0];
                Mostrar(""Top 1:"", primera.categoria, ""score:"", primera.score, ""pref:"", primera.preferencia);
                object output = { top: categorias };
            ");
        }

        //=======================================================================================================
        // DATOS EXTERNOS
        //=======================================================================================================

        public static Dictionary<string, object?> ExternalDataCliente()
        {
            return new Dictionary<string, object?>
            {
                ["cliente"] = new Dictionary<string, object?>
                {
                    ["id"] = 123,
                    ["nombre"] = "Juan",
                    ["saldo"] = 2500.3999,
                    ["activo"] = true
                },
                ["transacciones"] = new List<object?>
                {
                    new Dictionary<string, object?> { ["tipo"] = "deposito", ["monto"] = 500 },
                    new Dictionary<string, object?> { ["tipo"] = "retiro", ["monto"] = 200 }
                }
            };
        }

        public static Dictionary<string, object?> ExternalDataDescuentoClienteA()
        {
            return new Dictionary<string, object?>
            {
                ["precio"] = 1500.75,
                ["factor"] = 0.90,
                ["precio2"] = 800,
                ["factor2"] = 0.80
            };
        }

        public static Dictionary<string, object?> ExternalDataDescuentoClienteB()
        {
            return new Dictionary<string, object?>
            {
                ["precio"] = 500.75,
                ["factor"] = 0.60,
                ["precio2"] = 300,
                ["factor2"] = 0.70
            };
        }

        public static Dictionary<string, object?> ExternalDataPreferencia()
        {
            return new Dictionary<string, object?>
            {
                ["preferencia"] = "premium"
            };
        }
    }
}
