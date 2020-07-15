# Título del Proyecto

Desarrollo de aplicación web basada en FaaS con .NET Core. Evolución desde aplicación monolítica.

## Resumen

El presente Trabajo Fin de Máster se basa en el estudio de la evolución en el desarrollo desde una aplicación monolítica hasta una aplicación basada en funciones como servicio (FaaS). 

La aplicación que se desarrolla es una aplicación web sencilla con base de datos, destinada a la gestión de las reservas para distintos servicios. Su desarrollo se realiza utilizando distintas tecnologías englobadas en el framework .NET Core.

Con el objetivo de analizar la evolución de la aplicación, se ha tenido en cuenta el paso por distintas arquitecturas: monolítica, tres capas, microservicios y FaaS. Cada una de estas arquitecturas desarrolladas tendrá como base la arquitectura anterior, de esta forma se ve cada arquitectura como un paso hasta la arquitectura basada en FaaS. Siendo el resultado final cuatro aplicaciones, una por cada una de las arquitecturas planteadas.

A partir del desarrollo de estas cuatro aplicaciones se concluye con la comparativa de cada una de estas arquitecturas, analizando las ventajas y desventajas de las mismas para el desarrollo de la aplicación web. Siendo las métricas comparadas: La complejidad, el acoplamiento, la eficiencia organizativa y el rendimiento en cada una de estas arquitecturas.

## Estructura

Se ha creado una rama en github para cada una de las arquitecturas planteadas.
 
 - Monolítica
 - Microservices
 - ThreeLayers
 - FaaS
 
## Pre-requisitos

Disponer de Visual Studio.

## Comenzando 

En propiedades de la solución se debe establecer proyecto de inicio muútiples. Marcar iniciar todos los proyectos en cada una de las arquitecturas salvo el proyecto de acceso a datos.

Tras esto, solo es necesario darle a iniciar en Visual Studio.
