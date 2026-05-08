terraform {
  required_providers {
    docker = {
      source = "kreuzwerker/docker"
    }
  }
}


provider "docker" {}

resource "docker_image" "ubuntu" {
  name = "ubuntu:latest"
}

resource "docker_container" "server" {
  name  = "devops-server"
  image = docker_image.ubuntu.name

  tty   = true
  command = ["sleep", "3600"]

  ports {
    internal = 80
    external = 6000
  }
}