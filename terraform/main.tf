provider "aws" {
  region = "us-east-2"
}

resource "aws_eks_cluster" "fastfood_cluster" {
  name     = "ClusterEKSFastFoodApi"
  role_arn = "arn:aws:iam::626635438302:role/FunctionCreateViewUpdateAKS" # Substitua pelo ARN do seu role do EKS

  vpc_config {
    subnet_ids = ["subnet-0fc8ad18d5b8b3e04", "subnet-081a4e4b7e4a5f100", "subnet-0ace4dfb60596e3a9"] # Substitua pelos IDs das subnets do seu VPC
  }
}

resource "aws_eks_node_group" "fastfood_node_group" {
  cluster_name    = aws_eks_cluster.fastfood_cluster.name
  node_group_name = "fastfood-node-group"
  node_role_arn   = "arn:aws:iam::62663543830:role/EKSNodeGroupRole" # Substitua pelo ARN do seu role de nó

  subnet_ids = ["subnet-0fc8ad18d5b8b3e04", "subnet-081a4e4b7e4a5f100", "subnet-0ace4dfb60596e3a9"] # Substitua pelos IDs das subnets do seu VPC

  scaling_config {
    desired_size = 2
    max_size     = 3
    min_size     = 1
  }
}
