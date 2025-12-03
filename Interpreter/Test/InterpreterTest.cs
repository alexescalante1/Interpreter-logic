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
                NUMERIC numero = 42;
                STRING texto = ""Hola Mundo"";
                BOOL esVerdadero = true;
                BOOL esFalso = false;
                
                Mostrar(numero);
                Mostrar(texto);
                Mostrar(esVerdadero);
                Mostrar(esFalso);

                Mostrar(""=== TEST 2: ARITMETICA ==="");
                NUMERIC suma = 10 + 5;
                NUMERIC resta = 20 - 8;
                NUMERIC multiplicacion = 6 * 7;
                NUMERIC division = 100 / 4;
                NUMERIC modulo = 17 % 5;
                NUMERIC compleja = 10 + 5 * 2 - 8 / 4;
                
                Mostrar(suma);
                Mostrar(resta);
                Mostrar(multiplicacion);
                Mostrar(division);
                Mostrar(modulo);
                Mostrar(compleja);

                Mostrar(""=== TEST 3: COMPARACIONES ==="");
                NUMERIC a = 10;
                NUMERIC b = 20;
                
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
                BOOL verdad = true;
                BOOL mentira = false;
                
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
                NUMERIC puntuacion = 85;
                
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
                NUMERIC dia = 3;
                
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
                
                STRING color = ""rojo"";
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
                NUMERIC contador = 0;
                WHILE(contador < 5)
                {
                    Mostrar(contador);
                    contador = contador + 1;
                }
                
                NUMERIC factorial = 1;
                NUMERIC n = 5;
                NUMERIC i = 1;
                WHILE(i <= n)
                {
                    factorial = factorial * i;
                    i = i + 1;
                }
                Mostrar(""Factorial de 5:"");
                Mostrar(factorial);

                Mostrar(""=== TEST 8: FOR ==="");
                FOR(NUMERIC x = 0; x < 5; x = x + 1)
                {
                    Mostrar(x);
                }
                
                Mostrar(""Tabla del 5:"");
                FOR(NUMERIC j = 1; j <= 10; j = j + 1)
                {
                    NUMERIC resultadoTabla = 5 * j;
                    Mostrar(resultadoTabla);
                }

                Mostrar(""=== TEST 9: SCOPES ==="");
                NUMERIC global = 100;
                Mostrar(global);
                
                FOR(NUMERIC local = 0; local < 3; local = local + 1)
                {
                    NUMERIC dentroFor = 999;
                    Mostrar(local);
                    Mostrar(global);
                }

                Mostrar(""=== TEST 10: ANIDAMIENTO ==="");
                NUMERIC nivel1 = 1;
                
                IF(nivel1 == 1)
                {
                    NUMERIC nivel2 = 2;
                    Mostrar(nivel2);
                    
                    FOR(NUMERIC nivel3 = 0; nivel3 < 2; nivel3 = nivel3 + 1)
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
                STRING nombre = ""Juan"";
                STRING apellido = ""Perez"";
                STRING nombreCompleto = nombre + "" "" + apellido;
                Mostrar(nombreCompleto);
                
                STRING mensaje = ""Hola "" + nombre + "", tienes "" + 30 + "" años"";
                Mostrar(mensaje);

                Mostrar(""=== TEST 12: EXPRESIONES COMPLEJAS ==="");
                NUMERIC x1 = 10;
                NUMERIC y1 = 20;
                NUMERIC z1 = 30;
                
                IF((x1 < y1 AND y1 < z1) OR x1 == 10)
                {
                    Mostrar(""Expresion compleja evaluada correctamente"");
                }
                
                NUMERIC calculo = (10 + 20) * 2 - 15 / 3;
                Mostrar(calculo);

                Mostrar(""=== TEST 13: NEGACION ==="");
                NUMERIC positivo = 50;
                NUMERIC negativo = -positivo;
                Mostrar(negativo);
                
                NUMERIC resultadoNeg = -10 + 20;
                Mostrar(resultadoNeg);

                Mostrar(""=== TEST 14: REASIGNACION ==="");
                NUMERIC variable = 10;
                Mostrar(variable);
                
                variable = 20;
                Mostrar(variable);
                
                variable = variable + 5;
                Mostrar(variable);
                
                variable = variable * 2;
                Mostrar(variable);

                Mostrar(""=== TEST 15: BUCLES CONDICIONALES ==="");
                NUMERIC suma2 = 0;
                FOR(NUMERIC k = 1; k <= 10; k = k + 1)
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
                NUMERIC cero = 0;
                NUMERIC uno = 1;
                NUMERIC negativo1 = -1;
                
                Mostrar(""Multiplicacion por 0:"", 5 * cero);
                Mostrar(""Multiplicacion por 1:"", 5 * uno);
                Mostrar(""Multiplicacion por -1:"", 5 * negativo1);
                Mostrar(""Division por 1:"", 100 / uno);
                Mostrar(""Suma con negativos:"", 10 + negativo1);
                Mostrar(""Resta con negativos:"", 10 - negativo1);
                
                NUMERIC expresionCompleja = -5 * 3 + 20 / 4 - 2;
                Mostrar(""Expresion con negativos:"", expresionCompleja);

                Mostrar(""=== TEST 18: COMPARACIONES EXTREMAS ==="");
                NUMERIC minimo = -100;
                NUMERIC maximo = 100;
                NUMERIC medio = 0;
                
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
                STRING vacio = """";
                STRING espacios = ""   "";
                STRING numerosStr = ""12345"";
                STRING especiales = ""!@#$%"";
                
                Mostrar(""Cadena vacia:"", vacio, ""<fin"");
                Mostrar(""Espacios:"", espacios, ""<fin"");
                Mostrar(""Numeros como STRING:"", numerosStr);
                Mostrar(""Caracteres especiales:"", especiales);
                
                STRING concatenada = vacio + ""hola"" + espacios + ""mundo"";
                Mostrar(""Concatenacion compleja:"", concatenada);

                Mostrar(""=== TEST 20: LOGICA BOOLEANA AVANZADA ==="");
                BOOL a1 = true;
                BOOL b1 = false;
                BOOL c1 = true;
                
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
                
                FOR(NUMERIC testDia = 0; testDia <= 8; testDia = testDia + 1)
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
                NUMERIC a2 = 0;
                NUMERIC b2 = 10;
                
                WHILE(a2 < 5 AND b2 > 5)
                {
                    Mostrar(""a2:"", a2, ""b2:"", b2);
                    a2 = a2 + 1;
                    b2 = b2 - 1;
                }
                
                NUMERIC contador2 = 0;
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
                FOR(NUMERIC dec = 10; dec > 0; dec = dec - 1)
                {
                    Mostrar(""Cuenta regresiva:"", dec);
                }
                
                FOR(NUMERIC dec2 = 20; dec2 >= 10; dec2 = dec2 - 2)
                {
                    Mostrar(""Decremento de 2:"", dec2);
                }

                Mostrar(""=== TEST 24: SCOPES PROFUNDOS ==="");
                NUMERIC nivel0 = 0;
                
                IF(nivel0 == 0)
                {
                    NUMERIC nivel1Var = 1;
                    
                    IF(nivel1Var == 1)
                    {
                        NUMERIC nivel2Var = 2;
                        
                        IF(nivel2Var == 2)
                        {
                            NUMERIC nivel3Var = 3;
                            
                            IF(nivel3Var == 3)
                            {
                                NUMERIC nivel4Var = 4;
                                Mostrar(""Nivel 4 alcanzado:"", nivel4Var);
                                Mostrar(""Acceso a nivel 0:"", nivel0);
                            }
                        }
                    }
                }

                Mostrar(""=== TEST 25: MATEMATICAS COMPLEJAS ==="");
                NUMERIC resultado1 = ((10 + 5) * 3 - 8) / 2 + 10;
                Mostrar(""Resultado 1:"", resultado1);
                
                NUMERIC resultado2 = 100 - (50 / 2) + (20 * 3) - 15;
                Mostrar(""Resultado 2:"", resultado2);
                
                NUMERIC resultado3 = (((2 + 3) * 4) - 5) / 3;
                Mostrar(""Resultado 3:"", resultado3);
                
                NUMERIC a3 = 10;
                NUMERIC b3 = 20;
                NUMERIC c3 = 30;
                NUMERIC resultado4 = (a3 + b3) * c3 / (b3 - a3);
                Mostrar(""Resultado 4:"", resultado4);

                Mostrar(""=== TEST 26: CONDICIONES ENCADENADAS ==="");
                NUMERIC valor = 50;
                
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
                NUMERIC x2 = 0;
                NUMERIC y2 = 10;
                
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
                LIST miLista = [];

                ListAdd(miLista, 10);
                ListAdd(miLista, ""Hola"");
                ListAdd(miLista, true);
                ListAdd(miLista, 42);

                NUMERIC total = ListCount(miLista);
                Mostrar(""Total elementos:"", total);

                FOR(NUMERIC idx = 0; idx < ListCount(miLista); idx = idx + 1)
                {
                    Mostrar(""Elemento"", idx, "":"", miLista[idx]);
                }

                Mostrar(""=== TEST 28: LISTAS CON OPERACIONES ==="");
                LIST numerosLista = [];
                
                FOR(NUMERIC n1 = 1; n1 <= 5; n1 = n1 + 1)
                {
                    ListAdd(numerosLista, n1 * 10);
                }
                
                NUMERIC sumaLista = 0;
                FOR(NUMERIC n2 = 0; n2 < ListCount(numerosLista); n2 = n2 + 1)
                {
                    NUMERIC valorActual = numerosLista[n2];
                    sumaLista = sumaLista + valorActual;
                    Mostrar(""Elemento:"", valorActual, ""- Suma acumulada:"", sumaLista);
                }

                Mostrar(""=== TEST 29: LISTAS EDGE CASES ==="");
                LIST listaVacia = [];
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

                OBJECT persona = { nombre: ""Juan"", edad: 30, activo: true };

                Mostrar(""Nombre:"", persona.nombre);
                Mostrar(""Edad:"", persona.edad);
                Mostrar(""Activo:"", persona.activo);

                Mostrar(""=== TEST 30: LISTA DE OBJETOS ==="");
                LIST personas = [];
                ListAdd(personas, { nombre: ""Maria"", edad: 25 });
                ListAdd(personas, { nombre: ""Pedro"", edad: 35 });
                ListAdd(personas, { nombre: ""Ana"", edad: 28 });

                FOR(NUMERIC i = 0; i < ListCount(personas); i = i + 1)
                {
                    OBJECT p = personas[i];
                    Mostrar(""Persona:"", p.nombre, ""-"", p.edad);
                }

                Mostrar(""=== TEST 31: OBJETOS MIXTOS ==="");
                OBJECT producto = { 
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

                LIST productos = [];

                ListAdd(productos, { id: 1, nombre: ""Laptop"", precio: 1500, stock: 10 });
                ListAdd(productos, { id: 2, nombre: ""Mouse"", precio: 25, stock: 50 });
                ListAdd(productos, { id: 3, nombre: ""Teclado"", precio: 75, stock: 30 });
                ListAdd(productos, { id: 4, nombre: ""Monitor"", precio: 300, stock: 15 });
                ListAdd(productos, { id: 5, nombre: ""Webcam"", precio: 80, stock: 25 });

                FOR(NUMERIC i = 0; i < ListCount(productos); i = i + 1)
                {
                    OBJECT prod = productos[i];
                    Mostrar(""ID:"", prod.id, ""- Nombre:"", prod.nombre, ""- Precio:"", prod.precio, ""- Stock:"", prod.stock);
                }

                NUMERIC valorTotal = 0;
                FOR(NUMERIC i = 0; i < ListCount(productos); i = i + 1)
                {
                    OBJECT prod = productos[i];
                    NUMERIC valorProducto = prod.precio * prod.stock;
                    valorTotal = valorTotal + valorProducto;
                }
                Mostrar(""Valor total del inventario:"", valorTotal);

                Mostrar(""=== TEST 33: FILTRADO DE PRODUCTOS ==="");
                Mostrar(""Productos con stock bajo (menos de 20):"");
                
                FOR(NUMERIC i = 0; i < ListCount(productos); i = i + 1)
                {
                    OBJECT prod = productos[i];
                    IF(prod.stock < 20)
                    {
                        Mostrar(""ALERTA -"", prod.nombre, ""- Stock:"", prod.stock);
                    }
                }
                
                Mostrar(""Productos caros (mas de 100):"");
                FOR(NUMERIC i = 0; i < ListCount(productos); i = i + 1)
                {
                    OBJECT prod = productos[i];
                    IF(prod.precio > 100)
                    {
                        Mostrar(""PREMIUM -"", prod.nombre, ""- Precio:"", prod.precio);
                    }
                }

                Mostrar(""=== TEST 34: LISTA DE EMPLEADOS ==="");

                LIST empleados = [];

                ListAdd(empleados, { nombre: ""Juan Perez"", edad: 35, salario: 3000, activo: true });
                ListAdd(empleados, { nombre: ""Maria Lopez"", edad: 28, salario: 3500, activo: true });
                ListAdd(empleados, { nombre: ""Carlos Ruiz"", edad: 42, salario: 4000, activo: false });
                ListAdd(empleados, { nombre: ""Ana Torres"", edad: 31, salario: 3200, activo: true });
                ListAdd(empleados, { nombre: ""Luis Gomez"", edad: 45, salario: 4500, activo: false });

                Mostrar(""Empleados activos:"");
                NUMERIC contadorActivos = 0;
                FOR(NUMERIC i = 0; i < ListCount(empleados); i = i + 1)
                {
                    OBJECT emp = empleados[i];
                    IF(emp.activo == true)
                    {
                        Mostrar(""- "", emp.nombre, "" (Edad:"", emp.edad, "", Salario:"", emp.salario, "")"");
                        contadorActivos = contadorActivos + 1;
                    }
                }
                Mostrar(""Total empleados activos:"", contadorActivos);

                NUMERIC sumaTotal = 0;
                FOR(NUMERIC i = 0; i < ListCount(empleados); i = i + 1)
                {
                    OBJECT emp = empleados[i];
                    sumaTotal = sumaTotal + emp.salario;
                }
                NUMERIC promedio = sumaTotal / ListCount(empleados);
                Mostrar(""Salario promedio:"", promedio);

                Mostrar(""=== TEST 35: EMPLEADOS POR RANGO DE EDAD ==="");
                
                NUMERIC jovenes = 0;
                NUMERIC adultos = 0;
                NUMERIC seniors = 0;
                
                FOR(NUMERIC i = 0; i < ListCount(empleados); i = i + 1)
                {
                    OBJECT emp = empleados[i];
                    
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

                LIST transacciones = [];

                ListAdd(transacciones, { tipo: ""deposito"", monto: 1000, fecha: ""2025-01-15"" });
                ListAdd(transacciones, { tipo: ""retiro"", monto: 250, fecha: ""2025-01-16"" });
                ListAdd(transacciones, { tipo: ""deposito"", monto: 500, fecha: ""2025-01-17"" });
                ListAdd(transacciones, { tipo: ""retiro"", monto: 100, fecha: ""2025-01-18"" });
                ListAdd(transacciones, { tipo: ""deposito"", monto: 750, fecha: ""2025-01-19"" });
                ListAdd(transacciones, { tipo: ""retiro"", monto: 300, fecha: ""2025-01-20"" });

                NUMERIC balance = 0;
                NUMERIC totalDepositos = 0;
                NUMERIC totalRetiros = 0;
                NUMERIC cantidadDepositos = 0;
                NUMERIC cantidadRetiros = 0;
                
                FOR(NUMERIC i = 0; i < ListCount(transacciones); i = i + 1)
                {
                    OBJECT trans = transacciones[i];
    
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

                LIST pedidos = [];

                OBJECT cliente1 = { nombre: ""Juan"", telefono: ""123456"" };
                OBJECT producto1 = { nombre: ""Laptop"", precio: 1500 };

                ListAdd(pedidos, { 
                    id: 1, 
                    cliente: cliente1, 
                    producto: producto1, 
                    cantidad: 2 
                });

                OBJECT cliente2 = { nombre: ""Maria"", telefono: ""789012"" };
                OBJECT producto2 = { nombre: ""Mouse"", precio: 25 };

                ListAdd(pedidos, { 
                    id: 2, 
                    cliente: cliente2, 
                    producto: producto2, 
                    cantidad: 5 
                });

                FOR(NUMERIC i = 0; i < ListCount(pedidos); i = i + 1)
                {
                    OBJECT pedido = pedidos[i];
                    OBJECT cliente = pedido.cliente;
                    OBJECT producto3 = pedido.producto;
                    
                    Mostrar(""Pedido ID:"", pedido.id);
                    Mostrar(""Cliente:"", cliente.nombre, ""- Tel:"", cliente.telefono);
                    Mostrar(""Producto:"", producto3.nombre, ""- Precio:"", producto3.precio);
                    Mostrar(""Cantidad:"", pedido.cantidad);
                    
                    NUMERIC totalPedido = producto3.precio * pedido.cantidad;
                    Mostrar(""Total pedido:"", totalPedido);
                    Mostrar(""---"");
                }

                Mostrar(""=== TEST 38: BUSQUEDA EN LISTA ==="");

                LIST usuarios = [];
                ListAdd(usuarios, { id: 1, nombre: ""Juan"", email: ""juan@mail.com"" });
                ListAdd(usuarios, { id: 2, nombre: ""Maria"", email: ""maria@mail.com"" });
                ListAdd(usuarios, { id: 3, nombre: ""Pedro"", email: ""pedro@mail.com"" });
                ListAdd(usuarios, { id: 4, nombre: ""Ana"", email: ""ana@mail.com"" });

                NUMERIC buscarId = 2;
                BOOL encontrado = false;

                FOR(NUMERIC i = 0; i < ListCount(usuarios); i = i + 1)
                {
                    OBJECT user = usuarios[i];
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
                
                FOR(NUMERIC i = 0; i < ListCount(usuarios); i = i + 1)
                {
                    OBJECT user = usuarios[i];
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
                OBJECT config = { modo: ""normal"", nivel: 1, activo: false };
                
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
                
                LIST ventas = [];
                ListAdd(ventas, { producto: ""A"", cantidad: 10, precio: 100 });
                ListAdd(ventas, { producto: ""B"", cantidad: 5, precio: 200 });
                ListAdd(ventas, { producto: ""C"", cantidad: 15, precio: 50 });
                ListAdd(ventas, { producto: ""D"", cantidad: 8, precio: 150 });
                
                NUMERIC totalVentas = 0;
                NUMERIC cantidadTotal = 0;
                NUMERIC ventaMayor = 0;
                STRING productoMayor = """";
                
                FOR(NUMERIC i = 0; i < ListCount(ventas); i = i + 1)
                {
                    OBJECT venta = ventas[i];
                    NUMERIC montoVenta = venta.cantidad * venta.precio;
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
                OBJECT cliente = input.cliente;
                cliente.saldo = cliente.saldo + 100.01;
                OBJECT bonificacion = { tipo: ""bonificacion"", monto: 100 };
                ListAdd(input.transacciones, bonificacion);
                OBJECT output = input;
            ");
        }

        public static string ScriptControlFlow()
        {
            return TextMinifyHelper.Minify(@"
                Mostrar(""=== IF / ELSEIF / ELSE ==="");
                NUMERIC score = 82;
                IF(score >= 90) { Mostrar(""A""); }
                ELSEIF(score >= 80) { Mostrar(""B""); }
                ELSE { Mostrar(""C o menos""); }
                Mostrar(""=== SWITCH ==="");
                NUMERIC dia = 3;
                SWITCH(dia)
                {
                    CASE(1): Mostrar(""Lunes"");
                    CASE(2): Mostrar(""Martes"");
                    CASE(3): Mostrar(""Miercoles"");
                    DEFAULT: Mostrar(""Otro"");
                }
                Mostrar(""=== WHILE ==="");
                NUMERIC i = 0;
                WHILE(i < 3)
                {
                    Mostrar(""i ="", i);
                    i = i + 1;
                }
                Mostrar(""=== FOR ==="");
                FOR(NUMERIC j = 0; j < 3; j = j + 1)
                {
                    Mostrar(""j ="", j);
                }
                OBJECT output = { score: score, dia: dia };
            ");
        }

        public static string ScriptCollections()
        {
            return TextMinifyHelper.Minify(@"
                LIST numeros = [];
                ListAdd(numeros, 10);
                ListAdd(numeros, 20);
                ListAdd(numeros, 30);
                Mostrar(""Cantidad:"", ListCount(numeros));
                NUMERIC primero = numeros[0];
                Mostrar(""Primero:"", primero);
                OBJECT persona = { nombre: ""Ana"", edad: 28 };
                Mostrar(""Nombre:"", persona.nombre, ""Edad:"", persona.edad);
                persona.edad = persona.edad + 1;
                OBJECT output = { numeros: numeros, persona: persona };
            ");
        }

        public static string ScriptJson()
        {
            return TextMinifyHelper.Minify(@"
                OBJECT obj = { id: 1, nombre: ""Widget"", precio: 99.5 };
                obj.precio = obj.precio + 10;
                STRING jsonSalida = JsonStringify(obj);
                Mostrar(""JSON modificado:"", jsonSalida);
                OBJECT output = obj;
            ");
        }

        public static string ScriptJsonExterno()
        {
            return TextMinifyHelper.Minify(@"
                LIST descuentos = [];

                OBJECT req1 = { monto: input.precio, factor: input.factor };
                OBJECT res1 = AplicarDescuento(req1);
                ListAdd(descuentos, res1);
                Mostrar(""Descuento 1:"", res1.resultado);

                IF(res1.monto > 1000)
                {
                    OBJECT req2 = { monto: input.precio2, factor: input.factor2 * 2 };
                    OBJECT res2 = AplicarDescuento(req2);
                    ListAdd(descuentos, res2);
                    Mostrar(""Descuento 2:"", res2.resultado);
                }
                ELSE
                {
                    Mostrar(""Se omite segundo descuento"");
                }

                OBJECT output = { descuentos: descuentos };
            ");
        }

        public static string ScriptTopCategorias()
        {
            return TextMinifyHelper.Minify(@"
                LIST categorias = TopCategorias({ preferencia: input.preferencia });
                Mostrar(""Total categorias:"", ListCount(categorias));
                OBJECT primera = categorias[0];
                Mostrar(""Top 1:"", primera.categoria, ""score:"", primera.score, ""pref:"", primera.preferencia);
                OBJECT output = { top: categorias };
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
